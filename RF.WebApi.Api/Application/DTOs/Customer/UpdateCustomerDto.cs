using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.Customer
{
    public class UpdateCustomerDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(250)]
        public string CustomerName { get; set; } = string.Empty;

        [StringLength(20)]
        public string? PhoneNo { get; set; }

        [StringLength(250)]
        public string? Email { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }
    }
}
