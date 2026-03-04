using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.BusinessExpence
{
    public class CreateBusinessExpencePaymentDto
    {
        [Required(ErrorMessage = BusinessExpenceMessages.AmountRequired)]
        [Range(0.01, double.MaxValue, ErrorMessage = BusinessExpenceMessages.AmountPositive)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = BusinessExpenceMessages.PaymentAccountRequired)]
        public int PaymentAccountId { get; set; }
    }
}
