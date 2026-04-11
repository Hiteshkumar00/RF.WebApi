using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.Contribution
{
    public class AddContributionDto
    {
        public int Id { get; set; }
        public int? AccountPersonId { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
        public List<AddContributionPaymentDto>? Payments { get; set; }
    }

    public class CreateAddContributionDto
    {
        public int? AccountPersonId { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateOnly Date { get; set; }

        public List<CreateAddContributionPaymentDto>? Payments { get; set; }
    }

    public class UpdateAddContributionDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }

        public int? AccountPersonId { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateOnly Date { get; set; }

        public List<UpdateAddContributionPaymentDto>? Payments { get; set; }
    }

    public class AddContributionListDto
    {
        public int Id { get; set; }
        public int? AccountPersonId { get; set; }
        public string AccountPersonName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
