namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class SellingBillItem
    {
        public int? Id { get; set; }
        public string? ItemName { get; set; }
        public int? Quantity { get; set; } // Matches your "Quantity" spelling in the requirement
        public decimal? Price { get; set; }
        public int? BillId { get; set; } // Navigation property/FK for the parent bill

        public SellingItemWarrenty? Warrenty { get; set; }
    }
}
