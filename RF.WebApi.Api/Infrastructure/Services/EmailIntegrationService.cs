using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class EmailIntegrationService : IEmailIntegrationService
    {
        private readonly ILogger<EmailIntegrationService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public EmailIntegrationService(ILogger<EmailIntegrationService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task<ServiceResponse<bool>> SendBillAsync(SellingBill bill, Account account)
        {
            return await ServiceResponse<bool>.Execute(async err =>
            {
                if (!account.EnableEmail || string.IsNullOrEmpty(account.EmailSmtpHost))
                {
                    err.AddError("Email service is not enabled or configured in your profile.");
                    return false;
                }

                var targetEmail = bill.Email;
                if (string.IsNullOrEmpty(targetEmail))
                {
                    err.AddError("Customer email address is missing on the bill.");
                    return false;
                }

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

                // 2. Calculate Amounts for display
                var subtotal = bill.Items?.Sum(i => (i.Price ?? 0) * (i.Quantity ?? 0)) ?? 0;
                var totalAmount = subtotal - (bill.Discount ?? 0);
                var paidAmount = bill.Payments?.Sum(p => p.Amount ?? 0) ?? 0;
                var remainingAmount = totalAmount - paidAmount;
                var currency = account.CurrencyType ?? "₹";
                var dateStr = bill.Date?.ToString("dd-MMM-yyyy") ?? "N/A";

                // 3. Status Messaging
                string statusHeading = remainingAmount > 0 ? "Pending Payment Reminder" : "Payment Successful";
                string statusColor = remainingAmount > 0 ? "#e67e22" : "#27ae60"; 
                string statusText = remainingAmount > 0 
                    ? $"Your bill {bill.BillNo} has a pending balance. Please settle the amount at your earliest convenience."
                    : $"Your bill {bill.BillNo} has been successfully paid in full.";

                // 4. Prepare Email
                try
                {
                    using var smtpClient = new SmtpClient(account.EmailSmtpHost)
                    {
                        Port = account.EmailSmtpPort ?? 587,
                        Credentials = new NetworkCredential(account.EmailSmtpUsername, account.EmailSmtpPassword),
                        EnableSsl = account.EmailSmtpEnableSsl,
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(account.EmailSmtpUsername ?? "noreply@rf.com", account.ProfileName),
                        Subject = $"{statusHeading} - {bill.BillNo}",
                        Body = $@"
                            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #eee; border-radius: 10px; overflow: hidden;'>
                                <div style='background-color: {statusColor}; color: white; padding: 20px; text-align: center;'>
                                    <h2 style='margin: 0;'>{statusHeading}</h2>
                                </div>
                                <div style='padding: 20px;'>
                                    <h3 style='color: #2c3e50;'>{account.ProfileName}</h3>
                                    <p>Dear <strong>{bill.CustomerName}</strong>,</p>
                                    <p>{statusText}</p>
                                    
                                    <div style='background-color: #f9f9f9; padding: 15px; border-radius: 5px; border-left: 4px solid {statusColor};'>
                                        <p style='margin: 5px 0;'><strong>Bill Number:</strong> {bill.BillNo}</p>
                                        <p style='margin: 5px 0;'><strong>Date:</strong> {dateStr}</p>
                                        <hr style='border: 0; border-top: 1px solid #ddd;'/>
                                        <p style='margin: 5px 0;'><strong>Total Amount:</strong> {currency}{totalAmount:N2}</p>
                                        <p style='margin: 5px 0;'><strong>Paid Amount:</strong> {currency}{paidAmount:N2}</p>
                                        {(remainingAmount > 0 ? $"<p style='margin: 5px 0; color: #e74c3c;'><strong>Remaining Balance: {currency}{remainingAmount:N2}</strong></p>" : "<p style='margin: 5px 0; color: #27ae60;'><strong>Status: Fully Paid</strong></p>")}
                                    </div>
                                    
                                    <p style='margin-top: 20px;'>Please find the attached invoice for more details.</p>
                                    <p>If you have any questions, feel free to contact us.</p>
                                    <br/>
                                    <p style='margin: 0;'>Best Regards,</p>
                                    <p style='margin: 0;'><strong>{account.ProfileName}</strong></p>
                                    <p style='margin: 0; color: #7f8c8d; font-size: 0.9em;'>{account.Phone} | {account.Address}</p>
                                </div>
                            </div>",
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(targetEmail);

                    string fileName = $"Bill_{bill.BillNo}_{bill.Date:dd-MM-yyyy}_{bill.CustomerName}.pdf";
                    using var ms = new MemoryStream(pdfBytes);
                    var attachment = new Attachment(ms, fileName, "application/pdf");
                    mailMessage.Attachments.Add(attachment);

                    await smtpClient.SendMailAsync(mailMessage);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send email for Bill {BillId}", bill.Id);
                    err.AddError($"Email Send Error: {ex.Message}");
                    return false;
                }
            });
        }
    }
}
