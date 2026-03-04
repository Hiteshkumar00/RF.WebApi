using System;
using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.BusinessYear
{
    public class UpdateBusinessYearDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string YearName { get; set; } = string.Empty;

        [Required]
        public DateOnly Date { get; set; }
    }
}
