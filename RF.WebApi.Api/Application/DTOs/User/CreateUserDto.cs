namespace RF.WebApi.Api.Application.DTOs.User
{
    using System.ComponentModel.DataAnnotations;
    using RF.WebApi.Api.Domain.Common;

    public class CreateUserDto
    {
        public int? AccountId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string? PhoneNo { get; set; }

        [Required]
        [RegularExpression("^(SuperAdmin|Admin)$", UserMessages.InvalidRole]
        public string Role { get; set; } = "Admin";

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
