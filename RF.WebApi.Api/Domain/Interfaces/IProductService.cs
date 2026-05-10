using RF.WebApi.Api.Application.DTOs.Product;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResponse<int>> CreateProduct(CreateProductDto dto);
        Task<ServiceResponse<bool>> UpdateProduct(UpdateProductDto dto);
        Task<ServiceResponse<bool>> DeleteProduct(int id);
        Task<ServiceResponse<ProductDto>> GetProductById(int id);
        Task<ServiceResponse<List<ProductDto>>> GetAllProducts(ProductFilterDto filter);
        Task<ServiceResponse<List<ProductDto>>> GetProductSuggestions(string searchTerm);
    }
}
