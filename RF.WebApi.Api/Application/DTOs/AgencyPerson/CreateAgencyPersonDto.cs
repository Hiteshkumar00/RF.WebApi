using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.AgencyPerson
{
    public class CreateAgencyPersonDto
    {
        [Required(ErrorMessage = AgencyPersonMessages.AgencyRequired)]
        public int AgencyId { get; set; }

        [Required(ErrorMessage = AgencyPersonMessages.NameRequired)]
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;

        [Phone]
        [StringLength(50)]
        public string? PhoneNo { get; set; }

        [EmailAddress]
        [StringLength(250)]
        public string? Email { get; set; }

        [StringLength(250)]
        public string? PersonOccupation { get; set; }

        public string? Address { get; set; }
    }
}
