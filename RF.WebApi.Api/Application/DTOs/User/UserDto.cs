using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.User
{
    public class UserDto
    {
        [Required]
        public int Id { get; set; }
        public int? AccountId { get; set; }

        [Required(ErrorMessage = UserMessages.FirstNameRequired)]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        public string? MiddleName { get; set; }

        [Required(ErrorMessage = UserMessages.SurnameRequired)]
        [StringLength(100)]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = UserMessages.EmailRequired)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(50)]
        public string? PhoneNo { get; set; }

        [Required(ErrorMessage = UserMessages.RoleRequired)]
        [RegularExpression("^(SuperAdmin|Admin)$", ErrorMessage = UserMessages.InvalidRole)]
        public string Role { get; set; } = "Admin";

        [Required(ErrorMessage = UserMessages.IsActiveRequired)]
        public bool IsActive { get; set; }
    }
}