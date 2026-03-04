using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.BusinessYear
{
    public class ChangeUserSelectedYearDto
    {
        [Required]
        public int BusinessYearId { get; set; }
    }
}
