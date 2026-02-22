namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class BusinessYear
    {
        public int? Id { get; set; }
        public int? AccountId { get; set; }
        public string? YearName { get; set; }
        public DateOnly? Date { get; set; }
    }
}