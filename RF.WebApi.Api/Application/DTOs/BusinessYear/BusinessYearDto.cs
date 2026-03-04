using System;

namespace RF.WebApi.Api.Application.DTOs.BusinessYear
{
    public class BusinessYearDto
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public string YearName { get; set; } = string.Empty;
        public DateOnly? Date { get; set; }
    }
}
