using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.SellingBill
{
    // Warranty DTOs
    public class SellingItemWarrentyDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int BillId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
    }

    public class CreateSellingItemWarrentyDto
    {
        [Range(0, 50, ErrorMessage = SellingBillMessages.WarrantyYearPositive)]
        public int? Year { get; set; }

        [Range(0, 11, ErrorMessage = SellingBillMessages.WarrantyMonthPositive)]
        public int? Month { get; set; }

        [Range(0, 31, ErrorMessage = SellingBillMessages.WarrantyDayPositive)]
        public int? Day { get; set; }
    }

    public class UpdateSellingItemWarrentyDto : CreateSellingItemWarrentyDto
    {
        public int? Id { get; set; }
    }


    // Item DTOs
    public class SellingBillItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; } 
        public decimal Price { get; set; }

        public SellingItemWarrentyDto? Warrenty { get; set; }
    }

    public class CreateSellingBillItemDto
    {
        [Required(ErrorMessage = SellingBillMessages.ItemNameRequired)]
        [StringLength(250)]
        public string ItemName { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = SellingBillMessages.QuantityPositive)]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = SellingBillMessages.PricePositive)]
        public decimal Price { get; set; }

        // A single item can have at most one Warranty during creation
        public CreateSellingItemWarrentyDto? Warrenty { get; set; }
    }

    public class UpdateSellingBillItemDto : CreateSellingBillItemDto
    {
        public int? Id { get; set; }

        // Redefine to use the Update version for the nested property
        new public UpdateSellingItemWarrentyDto? Warrenty { get; set; }
    }


    // Payment DTOs
    public class SellingBillPaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int PaymentAccountId { get; set; }
    }

    public class CreateSellingBillPaymentDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = SellingBillMessages.AmountPositive)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = SellingBillMessages.PaymentAccountRequired)]
        public int PaymentAccountId { get; set; }
    }

    public class UpdateSellingBillPaymentDto : CreateSellingBillPaymentDto
    {
        public int? Id { get; set; }
    }
}
