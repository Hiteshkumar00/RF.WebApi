namespace RF.WebApi.Api.Application.DTOs.Customer
{
    public class CustomerListDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        // Financial & purchase aggregates
        public int TotalPurchases { get; set; }
        public decimal TotalNetAmount { get; set; }
        public decimal TotalRemainingAmount { get; set; }
    }
}
