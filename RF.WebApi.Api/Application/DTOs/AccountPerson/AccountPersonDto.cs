namespace RF.WebApi.Api.Application.DTOs.AccountPerson
{
    public class AccountPersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public string? PersonOccupation { get; set; }
        public string? Address { get; set; }
    }
}
