namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class BusinessExpence
    {
        public int? Id { get; set; }
        public int? AccountId { get; set; }
        public string? ExpenceType { get; set; }
        public DateOnly? Date { get; set; }
    }
}