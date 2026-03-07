using System;

namespace RF.WebApi.Api.Application.DTOs.BusinessYear
{
    public class BusinessYearDateRangeDto
    {
        public int Id { get; set; }
        public string YearName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
