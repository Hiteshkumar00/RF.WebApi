using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.Account
{
    public class CreateAccountDto
    {
        [Required]
        [StringLength(100)]
        public string ProfileName { get; set; } = string.Empty;

        public string? ProfileLogoLink { get; set; }

        public string? CurrencyType { get; set; }
    }
}
