using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.AccountPerson
{
    public class CreateAccountPersonDto
    {
        [Required]
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
