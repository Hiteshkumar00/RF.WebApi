namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class SellingItemWarrenty
    {
        public int? Id { get; set; }
        public int? ItemId { get; set; }
        public int? BillId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }

        // Navigation Properties
        public SellingBillItem? Item { get; set; }
        public SellingBill? Bill { get; set; }
    }
}