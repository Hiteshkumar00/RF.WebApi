using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.Contribution
{
    public class AddContributionPaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int PaymentAccountId { get; set; }
    }

    public class CreateAddContributionPaymentDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment Account is required.")]
        public int PaymentAccountId { get; set; }
    }

    public class UpdateAddContributionPaymentDto : CreateAddContributionPaymentDto
    {
        public int? Id { get; set; }
    }
}
