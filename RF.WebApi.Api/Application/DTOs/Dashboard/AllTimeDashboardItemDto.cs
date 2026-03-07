namespace RF.WebApi.Api.Application.DTOs.Dashboard
{
    public class AllTimeDashboardItemDto : DashboardDto
    {
        public int BusinessYearId { get; set; }
        public string BusinessYearName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
