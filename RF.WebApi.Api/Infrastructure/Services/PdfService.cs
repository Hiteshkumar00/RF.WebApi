using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RF.WebApi.Api.Application.Helpers;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using System.Collections.Generic;
using System.Linq;

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

        public byte[] GenerateSellingBillPdf(SellingBill bill, Account account)
        {
            var totalAmount = bill.Items.Sum(x => (x.Quantity ?? 0) * (x.Price ?? 0));
            var netAmount = totalAmount - (bill.Discount ?? 0);
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
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text(account.ProfileName).FontSize(22).ExtraBold().FontColor(Colors.Blue.Medium);
                            
                            if (!string.IsNullOrEmpty(account.Title))
                                col.Item().Text(account.Title).FontSize(10).FontColor(Colors.Grey.Medium);
                            
                            if (!string.IsNullOrEmpty(account.Address))
                                col.Item().PaddingTop(5).Text(account.Address).FontSize(9);
                            
                            var contactParts = new List<string>();
                            if (!string.IsNullOrEmpty(account.Phone)) contactParts.Add($"Phone: {account.Phone}");
                            if (!string.IsNullOrEmpty(account.Email)) contactParts.Add($"Email: {account.Email}");
                            
                            if (contactParts.Any())
                                col.Item().Text(string.Join(" | ", contactParts)).FontSize(9);

                            if (!string.IsNullOrEmpty(account.GSTIN))
                                col.Item().Text($"GSTIN: {account.GSTIN}").FontSize(9).SemiBold();
                        });

                        // Invoice Meta Details
                        row.ConstantItem(180).AlignRight().Column(col =>
                        {
                            col.Item().Text("TAX INVOICE").FontSize(18).ExtraBold().FontColor(Colors.BlueGrey.Darken2);
                            col.Item().PaddingTop(5).Text(text =>
                            {
                                text.Span("Invoice No: ").SemiBold();
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
                        // Customer & Status Information
                        col.Item().PaddingBottom(15).Row(row =>
                        {
                            // Billed To Box
                            row.RelativeItem().Border(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                            {
                                c.Item().Text("BILLED TO:").FontSize(9).SemiBold().FontColor(Colors.Grey.Darken2);
                                c.Item().Text(bill.CustomerName).FontSize(12).Bold();
                                c.Item().Text($"Phone: {bill.PhoneNo}");
                            });

                            row.ConstantItem(20); // Spacer

                            // Status Box
                            row.RelativeItem().Border(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                            {
                                c.Item().Text("PAYMENT STATUS:").FontSize(9).SemiBold().FontColor(Colors.Grey.Darken2);
                                if (remainingAmount <= 0)
                                    c.Item().Text("PAID").FontSize(16).ExtraBold().FontColor(Colors.Green.Medium);
                                else
                                    c.Item().Text("DUE").FontSize(16).ExtraBold().FontColor(Colors.Red.Medium);
                            });
                        });

                        // Items Table
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);
                                columns.RelativeColumn();
                                columns.ConstantColumn(50);
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(90);
                            });

                            // Styled Header
                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderStyle).Text("#");
                                header.Cell().Element(HeaderStyle).Text("Item Description");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("Qty");
                                header.Cell().Element(HeaderStyle).AlignRight().Text("Unit Price");
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
                                        c.Item().Text(item.ItemName).SemiBold();
                                        
                                        if (item.Warrenty != null && (item.Warrenty?.Year > 0 || item.Warrenty?.Month > 0 || item.Warrenty?.Day > 0))
                                        {
                                            var parts = new System.Collections.Generic.List<string>();
                                            if ((item.Warrenty.Year ?? 0) > 0) parts.Add($"{item.Warrenty.Year} Year{(item.Warrenty.Year > 1 ? "s" : "")}");
                                            if ((item.Warrenty.Month ?? 0) > 0) parts.Add($"{item.Warrenty.Month} Month{(item.Warrenty.Month > 1 ? "s" : "")}");
                                            if ((item.Warrenty.Day ?? 0) > 0) parts.Add($"{item.Warrenty.Day} Day{(item.Warrenty.Day > 1 ? "s" : "")}");

                                            if (parts.Count > 0)
                                            {
                                                c.Item().Text($"Warranty: {string.Join(", ", parts)}").FontSize(8).FontColor(Colors.Grey.Medium);
                                            }
                                        }
                                    });
                                table.Cell().Element(CellStyle).AlignCenter().Text(item.Quantity.ToString());
                                table.Cell().Element(CellStyle).AlignRight().Text((item.Price ?? 0).ToString("C2", culture));
                                table.Cell().Element(RowTotalStyle).AlignRight().Text(((item.Quantity ?? 0) * (item.Price ?? 0)).ToString("C2", culture));

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
                                totals.Item().PaddingVertical(2).Row(r => { r.RelativeItem().Text(t => t.Span("Discount:").FontColor(Colors.Orange.Darken1)); r.ConstantItem(100).AlignRight().Text(t => t.Span($"- {(bill.Discount ?? 0).ToString("C2", culture)}").FontColor(Colors.Orange.Darken1)); });
                                
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
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Thank you for your business! ").SemiBold();
                        x.Span($"This is a computer-generated invoice for {account.ProfileName}.").FontSize(8).FontColor(Colors.Grey.Medium);
                    });
                });
            }).GeneratePdf();
        }

        public byte[] GenerateBuyingBillPdf(BuyingBill bill, Account account)
        {
            var totalAmount = bill.Items.Sum(x => (x.Quantity ?? 0) * (x.Price ?? 0));
            var totalExpence = bill.Expences?.Sum(x => x.Amount ?? 0) ?? 0;
            var finalAmount = (totalAmount - (bill.Discount ?? 0)) + totalExpence;
            var paidAmount = bill.Payments.Sum(x => x.Amount ?? 0);
            var remainingAmount = finalAmount - paidAmount;
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
                        // Account / Internal Details
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text(account.ProfileName).FontSize(22).ExtraBold().FontColor(Colors.Blue.Medium);
                            
                            if (!string.IsNullOrEmpty(account.Title))
                                col.Item().Text(account.Title).FontSize(10).FontColor(Colors.Grey.Medium);
                            
                            if (!string.IsNullOrEmpty(account.Address))
                                col.Item().PaddingTop(5).Text(account.Address).FontSize(9);
                            
                            var contactParts = new List<string>();
                            if (!string.IsNullOrEmpty(account.Phone)) contactParts.Add($"Phone: {account.Phone}");
                            if (!string.IsNullOrEmpty(account.Email)) contactParts.Add($"Email: {account.Email}");
                            
                            if (contactParts.Any())
                                col.Item().Text(string.Join(" | ", contactParts)).FontSize(9);
                        });

                        // Document Meta Details
                        row.ConstantItem(180).AlignRight().Column(col =>
                        {
                            col.Item().Text("PURCHASE").FontSize(18).ExtraBold().FontColor(Colors.BlueGrey.Darken2);
                            col.Item().PaddingTop(5).Text(text =>
                            {
                                text.Span("Ref No: ").SemiBold();
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
                        // Vendor & Status Information
                        col.Item().PaddingBottom(15).Row(row =>
                        {
                            // Vendor Box
                            row.RelativeItem().Border(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                            {
                                c.Item().Text("AGENCY DETAILS:").FontSize(9).SemiBold().FontColor(Colors.Grey.Darken2);
                                c.Item().Text(bill.Agency?.AgencyName ?? "Unknown Vendor").FontSize(12).Bold().FontColor(Colors.BlueGrey.Darken2);
                            });

                            row.ConstantItem(20); // Spacer

                            // Payment Status Box
                            row.RelativeItem().Border(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
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
                                columns.ConstantColumn(50);
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(90);
                            });

                            // Styled Header
                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderStyle).Text("#");
                                header.Cell().Element(HeaderStyle).Text("Item Description");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("Qty");
                                header.Cell().Element(HeaderStyle).AlignRight().Text("Unit Cost");
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
                                table.Cell().Element(CellStyle).Text(item.ItemName).SemiBold();
                                table.Cell().Element(CellStyle).AlignCenter().Text(item.Quantity.ToString());
                                table.Cell().Element(CellStyle).AlignRight().Text((item.Price ?? 0).ToString("C2", culture));
                                table.Cell().Element(CellStyle).AlignRight().Text(((item.Quantity ?? 0) * (item.Price ?? 0)).ToString("C2", culture));

                                static IContainer CellStyle(IContainer container) =>
                                    container.BorderBottom(1).BorderColor(Colors.Grey.Lighten3).PaddingVertical(8).PaddingHorizontal(5);
                            }
                        });

                        // Expenses and Totals Area
                        col.Item().PaddingTop(15).Row(row =>
                        {
                            // Additional Expenses (Left Side)
                            row.RelativeItem().PaddingRight(20).Column(eCol =>
                            {
                                if (bill.Expences != null && bill.Expences.Count > 0)
                                {
                                    eCol.Item().PaddingBottom(5).Text(t => t.Span("Additional Expenses:").FontSize(10).SemiBold());
                                    eCol.Item().Table(eTable =>
                                    {
                                        eTable.ColumnsDefinition(cs => { cs.RelativeColumn(); cs.ConstantColumn(80); });
                                        foreach (var exp in bill.Expences)
                                        {
                                            eTable.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).PaddingVertical(4).Text(exp.ExpenceType).FontSize(9).FontColor(Colors.Grey.Darken2);
                                            eTable.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).PaddingVertical(4).AlignRight().Text((exp.Amount ?? 0).ToString("C2", culture)).FontSize(9);
                                        }
                                    });
                                }
                            });

                            // Totals Box (Right Side)
                            row.ConstantItem(260).Background(Colors.Grey.Lighten4).Padding(10).Column(totals =>
                            {
                                totals.Item().Row(r => { r.RelativeItem().Text(t => t.Span("Sub Total:").SemiBold()); r.ConstantItem(100).AlignRight().Text(t => t.Span(totalAmount.ToString("C2", culture))); });
                                
                                if ((bill.Discount ?? 0) > 0)
                                {
                                    totals.Item().PaddingVertical(2).Row(r => { r.RelativeItem().Text(t => t.Span("Discount:").FontColor(Colors.Orange.Darken1)); r.ConstantItem(100).AlignRight().Text(t => t.Span($"- {(bill.Discount ?? 0).ToString("C2", culture)}").FontColor(Colors.Orange.Darken1)); });
                                }

                                if (totalExpence > 0)
                                {
                                    totals.Item().PaddingVertical(2).Row(r => { r.RelativeItem().Text(t => t.Span("Other Expenses:")); r.ConstantItem(100).AlignRight().Text(t => t.Span(totalExpence.ToString("C2", culture))); });
                                }

                                totals.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                                
                                totals.Item().Row(r => { r.RelativeItem().Text(t => t.Span("FINAL AMOUNT:").SemiBold().FontSize(12).FontColor(Colors.BlueGrey.Darken2)); r.ConstantItem(100).AlignRight().Text(t => t.Span(finalAmount.ToString("C2", culture)).SemiBold().FontSize(12).FontColor(Colors.BlueGrey.Darken2)); });
                                
                                totals.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                                
                                totals.Item().Row(r => { r.RelativeItem().Text(t => t.Span("Paid to Vendor:").FontColor(Colors.Green.Darken1)); r.ConstantItem(100).AlignRight().Text(t => t.Span(paidAmount.ToString("C2", culture)).FontColor(Colors.Green.Darken1)); });
                                totals.Item().Row(r => { r.RelativeItem().Text(t => t.Span("Outstanding:").SemiBold().FontColor(Colors.Red.Medium)); r.ConstantItem(100).AlignRight().Text(t => t.Span(remainingAmount.ToString("C2", culture)).SemiBold().FontColor(Colors.Red.Medium)); });
                            });
                        });

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
                    page.Footer().AlignCenter().Text(x =>
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
