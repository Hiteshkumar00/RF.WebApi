namespace RF.WebApi.Api.Application.DTOs.Customer
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
