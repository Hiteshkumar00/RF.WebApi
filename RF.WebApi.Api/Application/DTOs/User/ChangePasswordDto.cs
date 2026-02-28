namespace RF.WebApi.Api.Application.DTOs.User
{
    using System.ComponentModel.DataAnnotations;

    public class ResetPasswordBySuperAdminDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
