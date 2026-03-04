using System;

namespace RF.WebApi.Api.Application.DTOs.BusinessYear
{
    public class BusinessYearListDto
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public string YearName { get; set; } = string.Empty;
        public DateOnly? Date { get; set; }
        
        public bool IsSelected { get; set; }
    }
}
