using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.Account
{
    public class UpdateAccountDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = AccountMessages.ProfileNameRequired)]
        [StringLength(100)]
        public string ProfileName { get; set; } = string.Empty;

        public string? ProfileLogoLink { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? GSTIN { get; set; }
        public string? CurrencyType { get; set; }
        public bool EnableSuggestions { get; set; }
    }
}
