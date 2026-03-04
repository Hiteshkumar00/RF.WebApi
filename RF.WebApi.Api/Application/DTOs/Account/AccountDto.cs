using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.Account
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string ProfileName { get; set; } = string.Empty;
        public string? ProfileLogoLink { get; set; }
        public string? CurrencyType { get; set; }
    }
}
