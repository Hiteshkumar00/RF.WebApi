namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class SellingBillItem
    {
        public int? Id { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; } // Matches your "Quantity" spelling in the requirement
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public int? BillId { get; set; } // Navigation property/FK for the parent bill

        public SellingBill? Bill { get; set; }
        public Product? Product { get; set; }
    }
}
