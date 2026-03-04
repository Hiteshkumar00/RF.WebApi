using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.Account
{
    public class UpdateAccountDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ProfileName { get; set; } = string.Empty;

        public string? ProfileLogoLink { get; set; }

        public string? CurrencyType { get; set; }
    }
}
