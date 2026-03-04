using System;
using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.BusinessYear
{
    public class UpdateBusinessYearDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = BusinessYearMessages.YearNameRequired)]
        [StringLength(100)]
        public string YearName { get; set; } = string.Empty;

        [Required(ErrorMessage = BusinessYearMessages.DateRequired)]
        public DateOnly Date { get; set; }
    }
}
