namespace RF.WebApi.Api.Application.DTOs.AgencyPerson
{
    public class AgencyPersonDto
    {
        public int Id { get; set; }
        public int AgencyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public string? PersonOccupation { get; set; }
        public string? Address { get; set; }
    }
}
