namespace RF.WebApi.Api.Application.DTOs.User
{
    using System.ComponentModel.DataAnnotations;
    using RF.WebApi.Api.Domain.Common;

    public class ResetPasswordBySuperAdminDto
    {
        [Required(ErrorMessage = UserMessages.UserIdRequired)]
        public int UserId { get; set; }

        [Required(ErrorMessage = UserMessages.NewPasswordRequired)]
        [StringLength(100, MinimumLength = 8)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
