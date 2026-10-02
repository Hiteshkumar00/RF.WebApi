using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.Product
{
    public class ImportProductDto
    {
        public int RowNo { get; set; }

        [Required]
        public string ProductName { get; set; } = string.Empty;
        public string? ImageLink { get; set; }
        public int? WarrantyYear { get; set; }
        public int? WarrantyMonth { get; set; }
        public int? WarrantyDay { get; set; }
    }
}
