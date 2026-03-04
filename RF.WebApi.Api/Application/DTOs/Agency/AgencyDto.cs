namespace RF.WebApi.Api.Application.DTOs.Agency
{
    public class AgencyDto
    {
        public int Id { get; set; }
        public string AgencyName { get; set; } = string.Empty;
        public string? Address { get; set; }
    }
}
