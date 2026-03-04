using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.BuyingBill
{
    // Buying Bill Item DTOs
    public class BuyingBillItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quntity { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateBuyingBillItemDto
    {
        [Required(ErrorMessage = BuyingBillMessages.ItemNameRequired)]
        [StringLength(250)]
        public string ItemName { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = BuyingBillMessages.QuantityPositive)]
        public int Quntity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = BuyingBillMessages.PricePositive)]
        public decimal Price { get; set; }
    }

    public class UpdateBuyingBillItemDto : CreateBuyingBillItemDto
    {
        public int? Id { get; set; } // Null if added during update
    }


    // Buying Bill Payment DTOs
    public class BuyingBillPaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int PaymentAccountId { get; set; }
    }

    public class CreateBuyingBillPaymentDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = BuyingBillMessages.AmountPositive)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = BuyingBillMessages.PaymentAccountRequired)]
        public int PaymentAccountId { get; set; }
    }

    public class UpdateBuyingBillPaymentDto : CreateBuyingBillPaymentDto
    {
        public int? Id { get; set; }
    }


    // Buying Bill Expence DTOs
    public class BuyingBillExpenceDto
    {
        public int Id { get; set; }
        public string ExpenceType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int PaymentAccountId { get; set; }
    }

    public class CreateBuyingBillExpenceDto
    {
        [Required(ErrorMessage = BuyingBillMessages.ExpenceTypeRequired)]
        [StringLength(250)]
        public string ExpenceType { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = BuyingBillMessages.AmountPositive)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = BuyingBillMessages.PaymentAccountRequired)]
        public int PaymentAccountId { get; set; }
    }

    public class UpdateBuyingBillExpenceDto : CreateBuyingBillExpenceDto
    {
        public int? Id { get; set; }
    }
}
