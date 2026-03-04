using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.Agency
{
    public class CreateAgencyDto
    {
        [Required(ErrorMessage = AgencyMessages.AgencyNameRequired)]
        [StringLength(250)]
        public string AgencyName { get; set; } = string.Empty;

        public string? Address { get; set; }
    }
}
