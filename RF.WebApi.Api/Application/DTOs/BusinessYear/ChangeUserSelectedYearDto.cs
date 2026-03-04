using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.BusinessYear
{
    public class ChangeUserSelectedYearDto
    {
        [Required(ErrorMessage = BusinessYearMessages.BusinessYearRequired)]
        public int BusinessYearId { get; set; }
    }
}
