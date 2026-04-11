using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.Contribution
{
    public class RemoveContributionDto
    {
        public int Id { get; set; }
        public int? AccountPersonId { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
        public List<RemoveContributionPaymentDto>? Payments { get; set; }
    }

    public class CreateRemoveContributionDto
    {
        public int? AccountPersonId { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateOnly Date { get; set; }

        public List<CreateRemoveContributionPaymentDto>? Payments { get; set; }
    }

    public class UpdateRemoveContributionDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }

        public int? AccountPersonId { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateOnly Date { get; set; }

        public List<UpdateRemoveContributionPaymentDto>? Payments { get; set; }
    }

    public class RemoveContributionListDto
    {
        public int Id { get; set; }
        public int? AccountPersonId { get; set; }
        public string AccountPersonName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
