namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class BusinessExpence
    {
        public int? Id { get; set; }
        public int? AccountId { get; set; }
        public string? ExpenceType { get; set; }
        public DateOnly? Date { get; set; }

        // The total declared amount for this expense
        public decimal? TotalAmount { get; set; }

        // Nullable: set when this expense originated from a Buying Bill
        public int? BuyingBillId { get; set; }
        public BuyingBill? BuyingBill { get; set; }

        public ICollection<BusinessExpencePayment> Payments { get; set; } = new List<BusinessExpencePayment>();
    }
}