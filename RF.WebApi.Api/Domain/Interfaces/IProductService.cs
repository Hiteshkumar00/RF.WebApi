using RF.WebApi.Api.Application.DTOs.Product;
using RF.WebApi.Api.Application.DTOs.Common;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResponse<int>> CreateProduct(CreateProductDto dto);
        Task<ServiceResponse<bool>> UpdateProduct(UpdateProductDto dto);
        Task<ServiceResponse<bool>> DeleteProduct(int id);
        Task<ServiceResponse<ProductDto>> GetProductById(int id);
        Task<ServiceResponse<PagedResult<ProductDto>>> GetAllProducts(TableLazyLoadEventDto request);
        Task<ServiceResponse<List<ProductDto>>> GetProductSuggestions(string? searchTerm, List<int>? includeIds = null);
        Task<ServiceResponse<byte[]>> ExportProducts(ProductFilterDto filter);
        Task<ServiceResponse<ImportResultDto>> ImportProducts(List<ImportProductDto> dtos);
    }
}
