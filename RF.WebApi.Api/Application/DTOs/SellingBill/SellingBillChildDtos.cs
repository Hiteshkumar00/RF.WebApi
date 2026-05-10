using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.SellingBill
{
    // Item DTOs
    public class SellingBillItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; } 
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }

    public class CreateSellingBillItemDto
    {
        [Required(ErrorMessage = "Product is required")]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = SellingBillMessages.QuantityPositive)]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = SellingBillMessages.PricePositive)]
        public decimal Price { get; set; }

        public decimal Discount { get; set; }
    }

    public class UpdateSellingBillItemDto : CreateSellingBillItemDto
    {
        public int? Id { get; set; }
    }


    // Payment DTOs
    public class SellingBillPaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int PaymentAccountId { get; set; }
        public DateOnly? Date { get; set; }
    }

    public class CreateSellingBillPaymentDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = SellingBillMessages.AmountPositive)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = SellingBillMessages.PaymentAccountRequired)]
        public int PaymentAccountId { get; set; }

        public DateOnly? Date { get; set; }
    }

    public class UpdateSellingBillPaymentDto : CreateSellingBillPaymentDto
    {
        public int? Id { get; set; }
    }
}
