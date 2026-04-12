namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class User
    {
        public int? Id { get; set; }
        public int? AccountId { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? Surname { get; set; }
        public string Email { get; set; }
        public string? PhoneNo { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public virtual Account? Account { get; set; }
    }
}