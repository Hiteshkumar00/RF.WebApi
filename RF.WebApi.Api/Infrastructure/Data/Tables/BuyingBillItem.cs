namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class BuyingBillItem
    {
        public int? Id { get; set; }
        public string? ItemName { get; set; }
        public int? Quantity { get; set; } // Matches your "Quantity" spelling in the image
        public decimal? Price { get; set; }
        public int? BillId { get; set; }
    }
}
