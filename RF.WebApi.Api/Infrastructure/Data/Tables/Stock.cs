using System;

namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class Stock
    {
        public int? Id { get; set; }
        public int? BuyingBillId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateOnly? Date { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? Discount { get; set; }

        public BuyingBill? BuyingBill { get; set; }
        public Product? Product { get; set; }
    }
}
