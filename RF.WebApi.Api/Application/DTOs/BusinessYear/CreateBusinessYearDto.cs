using System;
using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.BusinessYear
{
    public class CreateBusinessYearDto
    {
        [Required]
        [StringLength(100)]
        public string YearName { get; set; } = string.Empty;

        [Required]
        public DateOnly Date { get; set; }
    }
}
