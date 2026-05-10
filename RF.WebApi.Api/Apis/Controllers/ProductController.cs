using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.Product;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            var result = await _productService.CreateProduct(dto);
            return HandleResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductDto dto)
        {
            var result = await _productService.UpdateProduct(dto);
            return HandleResponse(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProduct(id);
            return HandleResponse(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetProductById(id);
            return HandleResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductFilterDto filter)
        {
            var result = await _productService.GetAllProducts(filter);
            return HandleResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSuggestions([FromQuery] string searchTerm)
        {
            var result = await _productService.GetProductSuggestions(searchTerm);
            return HandleResponse(result);
        }
    }
}
