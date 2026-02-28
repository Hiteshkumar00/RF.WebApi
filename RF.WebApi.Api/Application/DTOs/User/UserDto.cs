using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.User
{
    public class UserDto
    {
        [Required]
        public int Id { get; set; }
        public int? AccountId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        public string? MiddleName { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNo { get; set; }

        [Required]
        [RegularExpression("^(SuperAdmin|Admin)$", ErrorMessage = UserMessages.InvalidRole)]
        public string Role { get; set; } = "Admin";

        [Required]
        public bool IsActive { get; set; }
    }
}