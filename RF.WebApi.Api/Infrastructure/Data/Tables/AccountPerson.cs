namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class AccountPerson
    {
        public int? Id { get; set; }
        public int? AccountId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public string? PersonOccupation { get; set; }
        public string? Address { get; set; }
    }
}