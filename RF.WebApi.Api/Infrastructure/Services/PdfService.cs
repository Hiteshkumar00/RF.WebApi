using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using System.Linq;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        public byte[] GenerateSellingBillPdf(SellingBill bill, Account account)
        {
            var totalAmount = bill.Items.Sum(x => (x.Quantity ?? 0) * (x.Price ?? 0));
            var netAmount = totalAmount - (bill.Discount ?? 0);
            var paidAmount = bill.Payments.Sum(x => x.Amount ?? 0);
            var remainingAmount = netAmount - paidAmount;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Verdana));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text(account.ProfileName).FontSize(24).SemiBold().FontColor(Colors.Blue.Medium);
                            col.Item().Text("A Legacy of Quality Furniture").FontSize(9).Italic().FontColor(Colors.Grey.Medium);
                        });

                        row.RelativeItem().AlignRight().Column(col =>
                        {
                            col.Item().Text("INVOICE").FontSize(32).ExtraBold().FontColor(Colors.Blue.Medium);
                            col.Item().Text($"Bill No: {bill.BillNo}").SemiBold();
                            col.Item().Text($"Date: {bill.Date:dd-MMM-yyyy}");
                        });
                    });

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("BILLED TO").FontSize(8).SemiBold().FontColor(Colors.Grey.Medium);
                                c.Item().Text(bill.CustomerName).FontSize(14).SemiBold(); // Fixed casing CustomerName
                                c.Item().Text(bill.PhoneNo).FontColor(Colors.Grey.Darken2); // Fixed casing PhoneNo
                            });
                        });

                        col.Item().PaddingTop(25).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);
                                columns.RelativeColumn();
                                columns.ConstantColumn(70);
                                columns.ConstantColumn(100);
                                columns.ConstantColumn(100);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("#");
                                header.Cell().Element(CellStyle).Text("Item Description");
                                header.Cell().Element(CellStyle).AlignCenter().Text("Qty");
                                header.Cell().Element(CellStyle).AlignRight().Text("Price");
                                header.Cell().Element(CellStyle).AlignRight().Text("Total");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold())
                                                    .PaddingVertical(5)
                                                    .BorderBottom(1)
                                                    .BorderColor(Colors.Blue.Lighten2);
                                }
                            });

                            int index = 1;
                            foreach (var item in bill.Items)
                            {
                                table.Cell().Element(RowStyle).Text(index++.ToString());
                                table.Cell().Element(RowStyle).Column(c =>
                                {
                                    c.Item().Text(item.ItemName).SemiBold();
                                    if (item.Warrenty != null)
                                        c.Item().Text($"Warranty: {item.Warrenty.Year} Years").FontSize(8).Italic().FontColor(Colors.Grey.Medium);
                                });
                                table.Cell().Element(RowStyle).AlignCenter().Text(item.Quantity.ToString());
                                table.Cell().Element(RowStyle).AlignRight().Text((item.Price ?? 0).ToString("C2"));
                                table.Cell().Element(RowStyle).AlignRight().Text(((item.Quantity ?? 0) * (item.Price ?? 0)).ToString("C2"));

                                static IContainer RowStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten4).PaddingVertical(5);
                                }
                            }
                        });

                        col.Item().AlignRight().PaddingTop(20).Column(totals =>
                        {
                            totals.Item().Row(row =>
                            {
                                row.ConstantItem(150).Text("Total Amount:").SemiBold();
                                row.ConstantItem(100).AlignRight().Text(totalAmount.ToString("C2"));
                            });

                            totals.Item().Row(row =>
                            {
                                row.ConstantItem(150).Text("Discount:").SemiBold().FontColor(Colors.Orange.Medium);
                                row.ConstantItem(100).AlignRight().Text($"- {bill.Discount ?? 0:C2}").FontColor(Colors.Orange.Medium);
                            });

                            totals.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Blue.Lighten3);

                            totals.Item().Row(row =>
                            {
                                row.ConstantItem(150).Text("NET AMOUNT:").FontSize(14).ExtraBold().FontColor(Colors.Blue.Medium);
                                row.ConstantItem(100).AlignRight().Text(netAmount.ToString("C2")).FontSize(14).ExtraBold().FontColor(Colors.Blue.Medium);
                            });

                            totals.Item().PaddingTop(10).Row(row =>
                            {
                                row.ConstantItem(150).Text("Amount Paid:").FontColor(Colors.Green.Medium);
                                row.ConstantItem(100).AlignRight().Text(paidAmount.ToString("C2")).FontColor(Colors.Green.Medium);
                            });

                            totals.Item().Row(row =>
                            {
                                row.ConstantItem(150).Text("Balance Due:").SemiBold().FontColor(Colors.Red.Medium);
                                row.ConstantItem(100).AlignRight().Text(remainingAmount.ToString("C2")).SemiBold().FontColor(Colors.Red.Medium);
                            });
                        });
                    });

                    page.Footer().Column(col =>
                    {
                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        col.Item().PaddingTop(5).AlignCenter().Text(x =>
                        {
                            x.Span("Electronic invoice generated for ");
                            x.Span(account.ProfileName).SemiBold();
                        });
                        col.Item().AlignCenter().Text("Thank you for your business!").FontSize(8).Italic();
                    });
                });
            }).GeneratePdf();
        }

        public byte[] GenerateBuyingBillPdf(BuyingBill bill, Account account)
        {
            var totalAmount = bill.Items.Sum(x => (x.Quantity ?? 0) * (x.Price ?? 0));
            var totalExpence = bill.Expences.Sum(x => x.Amount ?? 0);
            var finalAmount = (totalAmount - (bill.Discount ?? 0)) + totalExpence;
            var paidAmount = bill.Payments.Sum(x => x.Amount ?? 0);
            var remainingAmount = finalAmount - paidAmount;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Verdana));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text(account.ProfileName).FontSize(24).SemiBold().FontColor(Colors.Grey.Darken3);
                            col.Item().Text("Internal Purchase Record").FontSize(9).Italic().FontColor(Colors.Grey.Medium);
                        });

                        row.RelativeItem().AlignRight().Column(col =>
                        {
                            col.Item().Text("PURCHASE").FontSize(32).ExtraBold().FontColor(Colors.Grey.Medium);
                            col.Item().Text($"Ref: {bill.BillNo}").SemiBold();
                            col.Item().Text($"Date: {bill.Date:dd-MMM-yyyy}");
                        });
                    });

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("VENDOR / AGENCY").FontSize(8).SemiBold().FontColor(Colors.Grey.Medium);
                                c.Item().Text(bill.Agency?.AgencyName ?? "Unknown").FontSize(14).SemiBold().FontColor(Colors.Blue.Medium);
                            });
                        });

                        col.Item().PaddingTop(25).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);
                                columns.RelativeColumn();
                                columns.ConstantColumn(70);
                                columns.ConstantColumn(100);
                                columns.ConstantColumn(100);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("#");
                                header.Cell().Element(CellStyle).Text("Item Description");
                                header.Cell().Element(CellStyle).AlignCenter().Text("Qty");
                                header.Cell().Element(CellStyle).AlignRight().Text("Unit Cost");
                                header.Cell().Element(CellStyle).AlignRight().Text("Total");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold())
                                                    .PaddingVertical(5)
                                                    .BorderBottom(1)
                                                    .BorderColor(Colors.Grey.Medium);
                                }
                            });

                            int index = 1;
                            foreach (var item in bill.Items)
                            {
                                table.Cell().Element(RowStyle).Text(index++.ToString());
                                table.Cell().Element(RowStyle).Text(item.ItemName).SemiBold();
                                table.Cell().Element(RowStyle).AlignCenter().Text(item.Quantity.ToString());
                                table.Cell().Element(RowStyle).AlignRight().Text((item.Price ?? 0).ToString("C2"));
                                table.Cell().Element(RowStyle).AlignRight().Text(((item.Quantity ?? 0) * (item.Price ?? 0)).ToString("C2"));

                                static IContainer RowStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten4).PaddingVertical(5);
                                }
                            }
                        });


                        if (bill.Expences != null && bill.Expences.Count > 0)
                        {
                            col.Item().PaddingTop(20).Column(eCol =>
                            {
                                eCol.Item().Text("ADDITIONAL EXPENSES").FontSize(8).SemiBold().FontColor(Colors.Grey.Medium);
                                eCol.Item().Table(eTable =>
                                {
                                    eTable.ColumnsDefinition(cs => { cs.RelativeColumn(); cs.ConstantColumn(100); });
                                    foreach (var exp in bill.Expences)
                                    {
                                        eTable.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).PaddingVertical(2).Text(exp.ExpenceType);
                                        eTable.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).PaddingVertical(2).AlignRight().Text((exp.Amount ?? 0).ToString("C2"));
                                    }
                                });
                            });
                        }

                        col.Item().AlignRight().PaddingTop(20).Column(totals =>
                        {
                            totals.Item().Row(row =>
                            {
                                row.ConstantItem(150).Text("Sub Total:").SemiBold();
                                row.ConstantItem(100).AlignRight().Text(totalAmount.ToString("C2"));
                            });

                            totals.Item().Row(row =>
                            {
                                row.ConstantItem(150).Text("Discount:").SemiBold().FontColor(Colors.Orange.Medium);
                                row.ConstantItem(100).AlignRight().Text($"- {bill.Discount ?? 0:C2}").FontColor(Colors.Orange.Medium);
                            });

                            totals.Item().Row(row =>
                            {
                                row.ConstantItem(150).Text("Other Expenses:").SemiBold();
                                row.ConstantItem(100).AlignRight().Text(totalExpence.ToString("C2"));
                            });

                            totals.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                            totals.Item().Row(row =>
                            {
                                row.ConstantItem(150).Text("FINAL AMOUNT:").FontSize(14).ExtraBold().FontColor(Colors.Cyan.Darken2);
                                row.ConstantItem(100).AlignRight().Text(finalAmount.ToString("C2")).FontSize(14).ExtraBold().FontColor(Colors.Cyan.Darken2);
                            });

                            totals.Item().PaddingTop(10).Row(row =>
                            {
                                row.ConstantItem(150).Text("Paid to Vendor:").FontColor(Colors.Green.Medium);
                                row.ConstantItem(100).AlignRight().Text(paidAmount.ToString("C2")).FontColor(Colors.Green.Medium);
                            });

                            totals.Item().Row(row =>
                            {
                                row.ConstantItem(150).Text("Outstanding:").SemiBold().FontColor(Colors.Red.Medium);
                                row.ConstantItem(100).AlignRight().Text(remainingAmount.ToString("C2")).SemiBold().FontColor(Colors.Red.Medium);
                            });
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Internal Document | Page ");
                        x.CurrentPageNumber();
                    });
                });
            }).GeneratePdf();
        }
    }
}
