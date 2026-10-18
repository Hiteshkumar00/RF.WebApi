using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.PaymentAccount
{
    public class ImportPaymentAccountDto
    {
        public int RowNo { get; set; }

        [Required(ErrorMessage = PaymentAccountMessages.MethodNameRequired)]
        [StringLength(100)]
        public string MethodName { get; set; } = string.Empty;
    }
}
