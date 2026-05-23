using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RF.WebApi.Api.Application.Helpers;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using System.Net.Http;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
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

        public async Task<byte[]> GenerateSellingBillPdf(SellingBill bill, Account account)
        {
            byte[] logoBytes = null;
            if (!string.IsNullOrWhiteSpace(account.ProfileLogoLink))
            {
                try
                {
                    using var client = new HttpClient();
                    logoBytes = await client.GetByteArrayAsync(account.ProfileLogoLink);
                }
                catch { }
            }

            var totalAmount = bill.Items.Sum(x => (x.Quantity ?? 0) * (x.Price ?? 0));
            var totalDiscount = bill.Items.Sum(x => (x.Quantity ?? 0) * (x.Discount ?? 0));
            var netAmount = totalAmount - totalDiscount;
            var paidAmount = bill.Payments.Sum(x => x.Amount ?? 0);
            var remainingAmount = netAmount - paidAmount;
            var culture = GetCurrencyCulture(account.CurrencyType);

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Arial));

                    // 1. HEADER
                    page.Header().PaddingBottom(20).Row(row =>
                    {
                        // Company Details
                        row.RelativeItem().Column((ColumnDescriptor col) =>
                        {
                            col.Item().Row(logoRow => 
                            {
                                if (logoBytes != null)
                                {
                                    logoRow.ConstantItem(60).PaddingRight(10).Image(logoBytes);
                                }
                                logoRow.RelativeItem().AlignMiddle().Column(innerCol => 
                                {
                                    innerCol.Item().Text(t => t.Span(account.ProfileName).FontSize(22).ExtraBold().FontColor(Colors.Blue.Medium));
                                    
                                    if (!string.IsNullOrEmpty(account.Title))
                                        innerCol.Item().Text(t => t.Span(account.Title).FontSize(10).FontColor(Colors.Grey.Medium));
                                });
                            });

                            if (!string.IsNullOrEmpty(account.Address))
                                col.Item().PaddingTop(5).Text(t => t.Span(account.Address).FontSize(9));
                            
                            if (!string.IsNullOrEmpty(account.Phone))
                                col.Item().Text(t => t.Span($"Phone: {account.Phone}").FontSize(9));
                            if (!string.IsNullOrEmpty(account.Email))
                                col.Item().Text(t => t.Span($"Email: {account.Email}").FontSize(9));
 
                            if (!string.IsNullOrEmpty(account.GSTIN))
                                col.Item().Text(t => t.Span($"GSTIN: {account.GSTIN}").FontSize(9).SemiBold());
                        });

                        // Invoice Meta Details
                        row.ConstantItem(180).AlignRight().Column(col =>
                        {
                            col.Item().PaddingTop(5).Text(text =>
                            {
                                text.Span("Bill No: ").SemiBold();
                                text.Span(bill.BillNo);
                            });
                            col.Item().Text(text =>
                            {
                                text.Span("Date: ").SemiBold();
                                text.Span($"{DateFormatHelper.Format(bill.Date, account.DateFormat)}");
                            });
                        });
                    });

                    // 2. CONTENT
                    page.Content().Column(col =>
                    {
                        // Customer Information
                        col.Item().PaddingBottom(15).Row(row =>
                        {
                            // Billed To Box
                            row.RelativeItem().Border(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                            {
                                c.Item().Text(t => t.Span("BILLED TO:").FontSize(9).SemiBold().FontColor(Colors.Grey.Darken2));
                                c.Item().Text(t => t.Span(bill.CustomerName).FontSize(12).Bold());
                                c.Item().Text($"Phone: {bill.PhoneNo}");
                            });
                        });

                        // Items Table
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);
                                columns.RelativeColumn();
                                                                columns.ConstantColumn(35);
                                columns.ConstantColumn(75);
                                columns.ConstantColumn(65);
                                columns.ConstantColumn(75);
                                columns.ConstantColumn(85);
                                
                                
                            });

                            // Styled Header
                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderStyle).Text("#");
                                header.Cell().Element(HeaderStyle).Text("Item Description");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("Qty");
                                header.Cell().Element(HeaderStyle).AlignRight().Text("Unit Price");
                                header.Cell().Element(HeaderStyle).AlignRight().Text("Discount");
                                header.Cell().Element(HeaderStyle).AlignRight().Text("Net Price");
                                header.Cell().Element(HeaderStyle).AlignRight().Text("Total");

                                static IContainer HeaderStyle(IContainer container) =>
                                    container.DefaultTextStyle(x => x.SemiBold().FontColor(Colors.Blue.Darken4))
                                        .Background(Colors.Blue.Lighten4)
                                        .PaddingVertical(8)
                                        .PaddingHorizontal(5);
                            });

                            // Table Rows
                            int index = 1;
                            foreach (var item in bill.Items)
                            {
                                table.Cell().Element(CellStyle).Text(index++.ToString());
                                    table.Cell().Element(CellStyle).Column(c =>
                                    {
                                        c.Item().Text(item.Product?.ProductName ?? "Unknown Product").SemiBold();
                                        
                                        if (item.Product != null && ((item.Product.WarrantyYear ?? 0) > 0 || (item.Product.WarrantyMonth ?? 0) > 0 || (item.Product.WarrantyDay ?? 0) > 0))
                                        {
                                            var parts = new System.Collections.Generic.List<string>();
                                            if ((item.Product.WarrantyYear ?? 0) > 0) parts.Add($"{item.Product.WarrantyYear} Year{(item.Product.WarrantyYear > 1 ? "s" : "")}");
                                            if ((item.Product.WarrantyMonth ?? 0) > 0) parts.Add($"{item.Product.WarrantyMonth} Month{(item.Product.WarrantyMonth > 1 ? "s" : "")}");
                                            if ((item.Product.WarrantyDay ?? 0) > 0) parts.Add($"{item.Product.WarrantyDay} Day{(item.Product.WarrantyDay > 1 ? "s" : "")}");

                                            if (parts.Count > 0)
                                            {
                                                c.Item().Text($"Warranty: {string.Join(", ", parts)}").FontSize(8).FontColor(Colors.Grey.Medium);
                                            }
                                        }
                                    });
                                table.Cell().Element(CellStyle).AlignCenter().Text(item.Quantity.ToString());
                                table.Cell().Element(CellStyle).AlignRight().Text((item.Price ?? 0).ToString("C2", culture));
                                table.Cell().Element(CellStyle).AlignRight().Text((item.Discount ?? 0).ToString("C2", culture));
                                table.Cell().Element(CellStyle).AlignRight().Text(((item.Price ?? 0) - (item.Discount ?? 0)).ToString("C2", culture));
                                table.Cell().Element(RowTotalStyle).AlignRight().Text(((item.Quantity ?? 0) * ((item.Price ?? 0) - (item.Discount ?? 0))).ToString("C2", culture));

                                static IContainer CellStyle(IContainer container) =>
                                    container.BorderBottom(1).BorderColor(Colors.Grey.Lighten3).PaddingVertical(8).PaddingHorizontal(5);

                                static IContainer RowTotalStyle(IContainer container) =>
                                    container.BorderBottom(1).BorderColor(Colors.Grey.Lighten3).PaddingVertical(8).PaddingHorizontal(5).DefaultTextStyle(x => x.SemiBold());
                            }
                        });

                        // 3. TOTALS & FOOTPRINT
                        col.Item().PaddingTop(15).Row(row =>
                        {
                            row.RelativeItem(); // Spacer instead of T&C

                            // Totals Box (Right Side)
                            row.ConstantItem(250).Background(Colors.Grey.Lighten4).Padding(10).Column(totals =>
                            {
                                totals.Item().Row(r => { r.RelativeItem().Text(t => t.Span("Subtotal:")); r.ConstantItem(100).AlignRight().Text(t => t.Span(totalAmount.ToString("C2", culture))); });
                                totals.Item().PaddingVertical(2).Row(r => { r.RelativeItem().Text(t => t.Span("Discount:").FontColor(Colors.Orange.Darken1)); r.ConstantItem(100).AlignRight().Text(t => t.Span($"- {totalDiscount.ToString("C2", culture)}").FontColor(Colors.Orange.Darken1)); });
                                
                                totals.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                                
                                totals.Item().Row(r => { r.RelativeItem().Text(t => t.Span("NET AMOUNT:").SemiBold().FontSize(12)); r.ConstantItem(100).AlignRight().Text(t => t.Span(netAmount.ToString("C2", culture)).SemiBold().FontSize(12)); });
                                
                                totals.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                                
                                totals.Item().Row(r => { r.RelativeItem().Text(t => t.Span("Amount Paid:").FontColor(Colors.Green.Darken1)); r.ConstantItem(100).AlignRight().Text(t => t.Span(paidAmount.ToString("C2", culture)).FontColor(Colors.Green.Darken1)); });
                                totals.Item().Row(r => { r.RelativeItem().Text(t => t.Span("Balance Due:").SemiBold().FontColor(Colors.Red.Medium)); r.ConstantItem(100).AlignRight().Text(t => t.Span(remainingAmount.ToString("C2", culture)).SemiBold().FontColor(Colors.Red.Medium)); });
                            });
                        });

                        // Authorized Signatory
                        col.Item().PaddingTop(60).Row(row => 
                        {
                            row.RelativeItem(); // Spacer
                            row.ConstantItem(150).AlignRight().Column(c =>
                            {
                                c.Item().LineHorizontal(1).LineColor(Colors.Black);
                                c.Item().AlignCenter().Text("Authorized Signatory").FontSize(9).SemiBold();
                            });
                        });
                    });

                    // 4. FOOTER
                    page.Footer().AlignCenter().Text((TextDescriptor x) =>
                    {
                        x.Span("Thank you for your business! ").SemiBold();
                        x.Span($"This is a computer-generated invoice for {account.ProfileName}.").FontSize(8).FontColor(Colors.Grey.Medium);
                    });
                });
            }).GeneratePdf();
        }

        public async Task<byte[]> GenerateBuyingBillPdf(BuyingBill bill, Account account, IEnumerable<BusinessExpence> expences)
        {
            byte[] logoBytes = null;
            if (!string.IsNullOrWhiteSpace(account.ProfileLogoLink))
            {
                try
                {
                    using var client = new HttpClient();
                    logoBytes = await client.GetByteArrayAsync(account.ProfileLogoLink);
                }
                catch { }
            }

            var expencesList = expences?.ToList() ?? new List<BusinessExpence>();
            var totalAmount = bill.Stocks.Sum(x => (x.Quantity ?? 0) * (x.PurchasePrice ?? 0));
            var totalDiscount = bill.Stocks.Sum(x => (x.Quantity ?? 0) * (x.Discount ?? 0));
            var totalExpence = expencesList.Sum(x => x.TotalAmount ?? 0);
            var finalAmount = totalAmount - totalDiscount;
            var paidAmount = bill.Payments.Sum(x => x.Amount ?? 0);
            var remainingAmount = finalAmount - paidAmount;
            var culture = GetCurrencyCulture(account.CurrencyType);

            return Document.Create((IDocumentContainer container) =>
            {
                container.Page((PageDescriptor page) =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Arial));

                    // 1. HEADER
                    page.Header().PaddingBottom(20).Row((RowDescriptor row) =>
                    {
                        // Account / Internal Details
                        row.RelativeItem().Column((ColumnDescriptor col) =>
                        {
                            col.Item().Row(logoRow => 
                            {
                                if (logoBytes != null)
                                {
                                    logoRow.ConstantItem(60).PaddingRight(10).Image(logoBytes);
                                }
                                logoRow.RelativeItem().AlignMiddle().Column(innerCol => 
                                {
                                    innerCol.Item().Text(account.ProfileName).FontSize(22).ExtraBold().FontColor(Colors.Blue.Medium);
                                    
                                    if (!string.IsNullOrEmpty(account.Title))
                                        innerCol.Item().Text(account.Title).FontSize(10).FontColor(Colors.Grey.Medium);
                                });
                            });

                            if (!string.IsNullOrEmpty(account.Address))
                                col.Item().PaddingTop(5).Text(account.Address).FontSize(9);
                            
                            if (!string.IsNullOrEmpty(account.Phone))
                                col.Item().Text($"Phone: {account.Phone}").FontSize(9);
                            if (!string.IsNullOrEmpty(account.Email))
                                col.Item().Text($"Email: {account.Email}").FontSize(9);
                        });

                        // Document Meta Details
                        row.ConstantItem(180).AlignRight().Column((ColumnDescriptor col) =>
                        {
                            col.Item().Text("PURCHASE").FontSize(18).ExtraBold().FontColor(Colors.BlueGrey.Darken2);
                            col.Item().PaddingTop(5).Text((TextDescriptor text) =>
                            {
                                text.Span("Ref No: ").SemiBold();
                                text.Span(bill.BillNo);
                            });
                            col.Item().Text((TextDescriptor text) =>
                            {
                                text.Span("Date: ").SemiBold();
                                text.Span($"{DateFormatHelper.Format(bill.Date, account.DateFormat)}");
                            });
                        });
                    });

                    // 2. CONTENT
                    page.Content().Column((ColumnDescriptor col) =>
                    {
                        // Vendor & Status Information
                        col.Item().PaddingBottom(15).Row((RowDescriptor row) =>
                        {
                            // Vendor Box
                            row.RelativeItem().Border(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(10).Column((ColumnDescriptor c) =>
                            {
                                c.Item().Text("AGENCY DETAILS:").FontSize(9).SemiBold().FontColor(Colors.Grey.Darken2);
                                c.Item().Text(bill.Agency?.AgencyName ?? "Unknown Vendor").FontSize(12).Bold().FontColor(Colors.BlueGrey.Darken2);
                            });

                            row.ConstantItem(20); // Spacer

                            // Payment Status Box
                            row.RelativeItem().Border(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(10).Column((ColumnDescriptor c) =>
                            {
                                c.Item().Text("ACCOUNTING STATUS:").FontSize(9).SemiBold().FontColor(Colors.Grey.Darken2);
                                if (remainingAmount <= 0)
                                    c.Item().Text("SETTLED").FontSize(16).ExtraBold().FontColor(Colors.Green.Medium);
                                else
                                    c.Item().Text("OUTSTANDING").FontSize(16).ExtraBold().FontColor(Colors.Red.Medium);
                            });
                        });

                        // Items Table
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);
                                columns.RelativeColumn();
                                                                columns.ConstantColumn(35);
                                columns.ConstantColumn(75);
                                columns.ConstantColumn(65);
                                columns.ConstantColumn(75);
                                columns.ConstantColumn(85);
                                
                                
                            });

                            // Styled Header
                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderStyle).Text("#");
                                header.Cell().Element(HeaderStyle).Text("Item Description");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("Qty");
                                header.Cell().Element(HeaderStyle).AlignRight().Text("Unit Cost");
                                header.Cell().Element(HeaderStyle).AlignRight().Text("Discount");
                                header.Cell().Element(HeaderStyle).AlignRight().Text("Net Cost");
                                header.Cell().Element(HeaderStyle).AlignRight().Text("Total");

                                static IContainer HeaderStyle(IContainer container) =>
                                    container.DefaultTextStyle(x => x.SemiBold().FontColor(Colors.Blue.Darken4))
                                        .Background(Colors.Blue.Lighten4)
                                        .PaddingVertical(8)
                                        .PaddingHorizontal(5);
                            });

                            // Table Rows
                            int index = 1;
                            foreach (var item in bill.Stocks)
                            {
                                table.Cell().Element(CellStyle).Text(index++.ToString());
                                table.Cell().Element(CellStyle).Text(item.Product?.ProductName ?? "Unknown Product").SemiBold();
                                table.Cell().Element(CellStyle).AlignCenter().Text(item.Quantity.ToString());
                                table.Cell().Element(CellStyle).AlignRight().Text((item.PurchasePrice ?? 0).ToString("C2", culture));
                                table.Cell().Element(CellStyle).AlignRight().Text((item.Discount ?? 0).ToString("C2", culture));
                                table.Cell().Element(CellStyle).AlignRight().Text(((item.PurchasePrice ?? 0) - (item.Discount ?? 0)).ToString("C2", culture));
                                table.Cell().Element(CellStyle).AlignRight().Text(((item.Quantity ?? 0) * ((item.PurchasePrice ?? 0) - (item.Discount ?? 0))).ToString("C2", culture));

                                static IContainer CellStyle(IContainer container) =>
                                    container.BorderBottom(1).BorderColor(Colors.Grey.Lighten3).PaddingVertical(8).PaddingHorizontal(5);
                            }
                        });

                        // Expenses and Totals Area
                        col.Item().PaddingTop(15).Row((RowDescriptor row) =>
                        {
                            // Left Side (Empty spacer)
                            row.RelativeItem().PaddingRight(20);

                            // Totals Box (Right Side)
                            row.ConstantItem(260).Background(Colors.Grey.Lighten4).Padding(10).Column((ColumnDescriptor totals) =>
                            {
                                totals.Item().Row((RowDescriptor r) => { r.RelativeItem().Text(t => t.Span("Sub Total:").SemiBold()); r.ConstantItem(100).AlignRight().Text(t => t.Span(totalAmount.ToString("C2", culture))); });
                                
                                if (totalDiscount > 0)
                                {
                                    totals.Item().PaddingVertical(2).Row((RowDescriptor r) => { r.RelativeItem().Text(t => t.Span("Discount:").FontColor(Colors.Orange.Darken1)); r.ConstantItem(100).AlignRight().Text(t => t.Span($"- {totalDiscount.ToString("C2", culture)}").FontColor(Colors.Orange.Darken1)); });
                                }
                                
                                totals.Item().Row((RowDescriptor r) => { r.RelativeItem().Text(t => t.Span("FINAL AMOUNT:").SemiBold().FontSize(12).FontColor(Colors.BlueGrey.Darken2)); r.ConstantItem(100).AlignRight().Text(t => t.Span(finalAmount.ToString("C2", culture)).SemiBold().FontSize(12).FontColor(Colors.BlueGrey.Darken2)); });
                                
                                totals.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                                
                                totals.Item().Row((RowDescriptor r) => { r.RelativeItem().Text(t => t.Span("Paid to Vendor:").FontColor(Colors.Green.Darken1)); r.ConstantItem(100).AlignRight().Text(t => t.Span(paidAmount.ToString("C2", culture)).FontColor(Colors.Green.Darken1)); });
                                totals.Item().Row((RowDescriptor r) => { r.RelativeItem().Text(t => t.Span("Outstanding:").SemiBold().FontColor(Colors.Red.Medium)); r.ConstantItem(100).AlignRight().Text(t => t.Span(remainingAmount.ToString("C2", culture)).SemiBold().FontColor(Colors.Red.Medium)); });
                            });
                        });

                        // 4. SEPARATE EXPENSE SECTION
                        if (expencesList != null && expencesList.Count > 0)
                        {
                            col.Item().PaddingTop(30).Column(eSection =>
                            {
                                eSection.Item().Background(Colors.Orange.Lighten5).Padding(10).Row(r =>
                                {
                                    r.RelativeItem().Text(t => t.Span("EXPENSE BREAKDOWN").FontSize(12).SemiBold().FontColor(Colors.Orange.Darken3));
                                    r.RelativeItem().AlignRight().Text(t =>
                                    {
                                        t.Span("Total Bill Related Expenses: ").FontSize(10);
                                        t.Span(totalExpence.ToString("C2", culture)).FontSize(10).Bold().FontColor(Colors.Orange.Darken3);
                                    });
                                });

                                eSection.Item().Table(eTable =>
                                {
                                    eTable.ColumnsDefinition(cs =>
                                    {
                                        cs.ConstantColumn(30);
                                        cs.RelativeColumn();
                                        cs.ConstantColumn(100);
                                        cs.ConstantColumn(100);
                                        cs.ConstantColumn(100);
                                    });

                                    eTable.Header(h =>
                                    {
                                        h.Cell().Element(ExpHeaderStyle).Text("#");
                                        h.Cell().Element(ExpHeaderStyle).Text("Expense Type");
                                        h.Cell().Element(ExpHeaderStyle).AlignRight().Text("Declared");
                                        h.Cell().Element(ExpHeaderStyle).AlignRight().Text("Paid");
                                        h.Cell().Element(ExpHeaderStyle).AlignRight().Text("Remaining");

                                        static IContainer ExpHeaderStyle(IContainer container) =>
                                            container.DefaultTextStyle(x => x.SemiBold().FontSize(9).FontColor(Colors.Orange.Darken4))
                                                .BorderBottom(1).BorderColor(Colors.Orange.Lighten2)
                                                .PaddingVertical(5).PaddingHorizontal(5);
                                    });

                                    int eIndex = 1;
                                    foreach (var exp in expencesList)
                                    {
                                        var ePaid = exp.Payments.Sum(p => p.Amount ?? 0);
                                        var eRemaining = (exp.TotalAmount ?? 0) - ePaid;

                                        eTable.Cell().Element(ExpCellStyle).Text(eIndex++.ToString());
                                        eTable.Cell().Element(ExpCellStyle).Text(exp.ExpenceType).SemiBold();
                                        eTable.Cell().Element(ExpCellStyle).AlignRight().Text((exp.TotalAmount ?? 0).ToString("C2", culture));
                                        eTable.Cell().Element(ExpCellStyle).AlignRight().Text(ePaid.ToString("C2", culture)).FontColor(Colors.Green.Medium);
                                        eTable.Cell().Element(ExpCellStyle).AlignRight().Text(eRemaining.ToString("C2", culture)).FontColor(eRemaining > 0 ? Colors.Red.Medium : Colors.Green.Medium);

                                        static IContainer ExpCellStyle(IContainer container) =>
                                            container.BorderBottom(1).BorderColor(Colors.Grey.Lighten4).PaddingVertical(5).PaddingHorizontal(5).DefaultTextStyle(x => x.FontSize(9));
                                    }
                                });

                                // Expense Summary Totals
                                var totalExpPaid = expencesList.Sum(e => e.Payments.Sum(p => p.Amount ?? 0));
                                var totalExpRem = totalExpence - totalExpPaid;

                                eSection.Item().AlignRight().PaddingTop(5).Row(r =>
                                {
                                    r.ConstantItem(300).Column(c =>
                                    {
                                        c.Item().Row(tr =>
                                        {
                                            tr.RelativeItem().Text(t => t.Span("Total Paid Expenses:").FontSize(9).FontColor(Colors.Green.Darken2));
                                            tr.ConstantItem(100).AlignRight().Text(t => t.Span(totalExpPaid.ToString("C2", culture)).FontSize(9).FontColor(Colors.Green.Darken2));
                                        });
                                        c.Item().Row(tr =>
                                        {
                                            tr.RelativeItem().Text(t => t.Span("Total Remaining Expenses:").FontSize(9).SemiBold().FontColor(Colors.Red.Darken2));
                                            tr.ConstantItem(100).AlignRight().Text(t => t.Span(totalExpRem.ToString("C2", culture)).FontSize(9).SemiBold().FontColor(Colors.Red.Darken2));
                                        });
                                    });
                                });
                            });
                        }

                        // Signatures
                        col.Item().PaddingTop(60).Row(row => 
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Width(150).LineHorizontal(1).LineColor(Colors.Black);
                                c.Item().Text("Prepared By").FontSize(9).SemiBold();
                            });
                            row.ConstantItem(150).AlignRight().Column(c =>
                            {
                                c.Item().LineHorizontal(1).LineColor(Colors.Black);
                                c.Item().AlignCenter().Text("Authorized Signatory").FontSize(9).SemiBold();
                            });
                        });
                    });

                    // 3. FOOTER
                    page.Footer().AlignCenter().Text((TextDescriptor x) =>
                    {
                        x.Span("Internal Document | Page ").FontSize(9).FontColor(Colors.Grey.Medium);
                        x.CurrentPageNumber().FontSize(9).FontColor(Colors.Grey.Medium);
                        x.Span(" of ").FontSize(9).FontColor(Colors.Grey.Medium);
                        x.TotalPages().FontSize(9).FontColor(Colors.Grey.Medium);
                    });
                });
            }).GeneratePdf();
        }
    }
}
