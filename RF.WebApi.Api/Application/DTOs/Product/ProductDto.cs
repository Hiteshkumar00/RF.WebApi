using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageLink { get; set; }
        public int? WarrantyYear { get; set; }
        public int? WarrantyMonth { get; set; }
        public int? WarrantyDay { get; set; }
    }

    public class CreateProductDto
    {
        [Required]
        public string ProductName { get; set; } = string.Empty;
        public string? ImageLink { get; set; }
        public int? WarrantyYear { get; set; }
        public int? WarrantyMonth { get; set; }
        public int? WarrantyDay { get; set; }
    }

    public class UpdateProductDto : CreateProductDto
    {
        public int Id { get; set; }
    }

    public class ProductFilterDto
    {
        public string? SearchTerm { get; set; }
    }
}
