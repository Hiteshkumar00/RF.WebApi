using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.Account
{
    public class CreateAccountDto
    {
        [Required(ErrorMessage = AccountMessages.ProfileNameRequired)]
        [StringLength(100)]
        public string ProfileName { get; set; } = string.Empty;

        public string? ProfileLogoLink { get; set; }

        public string? CurrencyType { get; set; }
    }
}
