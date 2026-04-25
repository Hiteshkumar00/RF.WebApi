using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using RF.WebApi.Api.Application.Helpers;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class WhatsAppIntegrationService : IWhatsAppIntegrationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WhatsAppIntegrationService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public WhatsAppIntegrationService(HttpClient httpClient, ILogger<WhatsAppIntegrationService> logger, IServiceProvider serviceProvider)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        private System.Globalization.CultureInfo GetCurrencyCulture(string? currencyType)
        {
            try
            {
                return new System.Globalization.CultureInfo(string.IsNullOrWhiteSpace(currencyType) ? "en-IN" : currencyType);
            }
            catch
            {
                return new System.Globalization.CultureInfo("en-IN");
            }
        }

        public async Task<ServiceResponse<bool>> SendBillAsync(SellingBill bill, Account account)
        {
            return await ServiceResponse<bool>.Execute(async err =>
            {
                if (string.IsNullOrEmpty(account.WhatsAppPhoneNumberId) || string.IsNullOrEmpty(account.WhatsAppAccessToken))
                {
                    err.AddError("WhatsApp API credentials are not configured.");
                    return false;
                }

                var phone = bill.PhoneNo?.Replace("+", "").Replace("-", "").Replace(" ", "");
                if (string.IsNullOrEmpty(phone))
                {
                    err.AddError("Customer phone number is missing.");
                    return false;
                }
                if (phone.Length == 10) phone = "91" + phone;

                // 1. Generate PDF Bytes
                using var scope = _serviceProvider.CreateScope();
                var sellingBillService = scope.ServiceProvider.GetRequiredService<ISellingBillService>();
                var pdfResponse = await sellingBillService.GenerateInvoicePdf(bill.Id ?? 0);
                if (!pdfResponse.Success)
                {
                    err.SetErrors(pdfResponse);
                    return false;
                }
                var pdfBytes = pdfResponse.Data;

                // 2. Upload Media to WhatsApp Meta API
                var dateStrForFile = bill.Date?.ToString("dd-MM-yyyy") ?? DateTime.Now.ToString("dd-MM-yyyy");
                var dateStrForTemplate = DateFormatHelper.Format(bill.Date, account.DateFormat);
                string customerName = bill.CustomerName?.Replace(" ", "_") ?? "Customer";
                string fileName = $"Bill_{bill.BillNo}_{dateStrForFile}_{customerName}.pdf";

                var mediaId = await UploadMediaAsync(account.WhatsAppPhoneNumberId, account.WhatsAppAccessToken, pdfBytes, fileName);
                if (string.IsNullOrEmpty(mediaId))
                {
                    err.AddError("Failed to upload bill PDF to WhatsApp media server.");
                    return false;
                }

                // 3. Send Template Message with Document Header
                var url = $"https://graph.facebook.com/v19.0/{account.WhatsAppPhoneNumberId}/messages";
                
                var totalAmount = (bill.Items?.Sum(i => (i.Price ?? 0) * (i.Quantity ?? 0)) ?? 0) - (bill.Discount ?? 0);
                var paidAmount = bill.Payments?.Sum(p => p.Amount ?? 0) ?? 0;
                var remainingAmount = totalAmount - paidAmount;
                var culture = GetCurrencyCulture(account.CurrencyType);
                var currency = culture.NumberFormat.CurrencySymbol;

                var payload = new
                {
                    messaging_product = "whatsapp",
                    to = phone,
                    type = "template",
                    template = new
                    {
                        name = "selling_bill_reminder",
                        language = new { code = "en_US" },
                        components = new object[]
                        {
                            new {
                                type = "header",
                                parameters = new[] {
                                    new { type = "document", document = new { id = mediaId, filename = fileName } }
                                }
                            },
                            new {
                                type = "body",
                                parameters = new[] {
                                    new { type = "text", text = bill.CustomerName ?? "Customer" },
                                    new { type = "text", text = bill.BillNo ?? bill.Id.ToString() },
                                    new { type = "text", text = dateStrForTemplate },
                                    new { type = "text", text = $"{currency}{totalAmount:N2}" },
                                    new { type = "text", text = $"{currency}{remainingAmount:N2}" }
                                }
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(payload);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", account.WhatsAppAccessToken);
                
                var response = await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    err.AddError($"WhatsApp Send Error: {responseContent}");
                    return false;
                }

                return true;
            });
        }

        private async Task<string?> UploadMediaAsync(string phoneNumberId, string accessToken, byte[] fileBytes, string fileName)
        {
            try
            {
                var url = $"https://graph.facebook.com/v19.0/{phoneNumberId}/media";
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var content = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
                
                content.Add(new StringContent("whatsapp"), "messaging_product");
                content.Add(fileContent, "file", fileName);
                
                request.Content = content;

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) return null;

                using var doc = JsonDocument.Parse(responseContent);
                return doc.RootElement.GetProperty("id").GetString();
            }
            catch { return null; }
        }
    }
}
