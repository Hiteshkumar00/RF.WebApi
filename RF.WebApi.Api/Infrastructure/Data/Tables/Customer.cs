using System.Collections.Generic;

namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class Customer
    {
        public int? Id { get; set; }
        public int? AccountId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public ICollection<SellingBill> SellingBills { get; set; } = new List<SellingBill>();
    }
}
