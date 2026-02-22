namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class AgencyPerson
    {
        public int? Id { get; set; }
        public int? AgencyId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public string? PersonOccupation { get; set; }
        public string? Address { get; set; }
    }
}