using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.PaymentAccount
{
    public class UpdatePaymentAccountDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = PaymentAccountMessages.MethodNameRequired)]
        [StringLength(250)]
        public string MethodName { get; set; } = string.Empty;

        public int? AccountPersonId { get; set; }
    }
}
