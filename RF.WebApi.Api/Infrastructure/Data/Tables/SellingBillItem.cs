namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class SellingBillItem
    {
        public int? Id { get; set; }
        public string? ItemName { get; set; }
        public int? Quntity { get; set; } // Matches your "Quntity" spelling in the requirement
        public decimal? Price { get; set; }
        public int? BillId { get; set; } // Navigation property/FK for the parent bill
    }
}
