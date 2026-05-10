using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.BuyingBill
{
    // Stock DTOs
    public class StockDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Discount { get; set; }
        public DateOnly? Date { get; set; }
    }

    public class CreateStockDto
    {
        [Required(ErrorMessage = "Product is required")]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = BuyingBillMessages.QuantityPositive)]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = BuyingBillMessages.PricePositive)]
        public decimal PurchasePrice { get; set; }
        
        public decimal Discount { get; set; }
        
        public DateOnly? Date { get; set; }
    }

    public class UpdateStockDto : CreateStockDto
    {
        public int? Id { get; set; } // Null if added during update
    }


    // Buying Bill Payment DTOs
    public class BuyingBillPaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int PaymentAccountId { get; set; }
        public DateOnly? Date { get; set; }
    }

    public class CreateBuyingBillPaymentDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = BuyingBillMessages.AmountPositive)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = BuyingBillMessages.PaymentAccountRequired)]
        public int PaymentAccountId { get; set; }

        public DateOnly? Date { get; set; }
    }

    public class UpdateBuyingBillPaymentDto : CreateBuyingBillPaymentDto
    {
        public int? Id { get; set; }
    }


    // Buying Bill Expence DTOs
    public class BuyingBillExpencePaymentDto
    {
        public int? Id { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = BuyingBillMessages.AmountPositive)]
        public decimal Amount { get; set; }
        
        [Required(ErrorMessage = BuyingBillMessages.PaymentAccountRequired)]
        public int PaymentAccountId { get; set; }
 
        public DateOnly? Date { get; set; }
    }

    public class BuyingBillExpenceDto
    {
        public int Id { get; set; }
        public string ExpenceType { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<BuyingBillExpencePaymentDto> Payments { get; set; } = new();
    }

    public class CreateBuyingBillExpenceDto
    {
        [Required(ErrorMessage = BuyingBillMessages.ExpenceTypeRequired)]
        [StringLength(250)]
        public string ExpenceType { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = BuyingBillMessages.AmountPositive)]
        public decimal TotalAmount { get; set; }

        public List<BuyingBillExpencePaymentDto> Payments { get; set; } = new();
    }

    public class UpdateBuyingBillExpenceDto : CreateBuyingBillExpenceDto
    {
        public int? Id { get; set; }
    }
}
