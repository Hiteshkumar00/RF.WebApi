using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.Product;
using RF.WebApi.Api.Application.DTOs.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;

        public ProductService(RFDBContext context, IMapper mapper, IExcelService excelService) : base(context)
        {
            _mapper = mapper;
            _excelService = excelService;
        }

        public async Task<ServiceResponse<int>> CreateProduct(CreateProductDto dto)
        {
            return await ServiceResponse<int>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                // Check for duplicate name
                var exists = await _context.Products.AnyAsync(p => p.AccountId == accountId && p.ProductName == dto.ProductName);
                if (exists)
                {
                    err.AddError("Product name already exists.");
                    return default;
                }

                var product = _mapper.Map<Product>(dto);
                product.AccountId = accountId;

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return product.Id ?? default;
            });
        }

        public Task<ServiceResponse<bool>> UpdateProduct(UpdateProductDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == dto.Id && p.AccountId == accountId);
                if (product == null)
                {
                    err.AddError("Product not found.");
                    return false;
                }

                // Check for duplicate name excluding self
                var exists = await _context.Products.AnyAsync(p => p.AccountId == accountId && p.ProductName == dto.ProductName && p.Id != dto.Id);
                if (exists)
                {
                    err.AddError("Product name already exists.");
                    return false;
                }

                _mapper.Map(dto, product);
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public Task<ServiceResponse<bool>> DeleteProduct(int id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.AccountId == accountId);
                if (product == null)
                {
                    err.AddError("Product not found.");
                    return false;
                }

                // Check if product is used in stock or selling bills
                var isUsedInStock = await _context.Stocks.AnyAsync(s => s.ProductId == id);
                var isUsedInSelling = await _context.SellingBillItems.AnyAsync(s => s.ProductId == id);

                if (isUsedInStock || isUsedInSelling)
                {
                    err.AddError("Product cannot be deleted as it is linked to bills.");
                    return false;
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public Task<ServiceResponse<ProductDto>> GetProductById(int id)
        {
            return ServiceResponse<ProductDto>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id && p.AccountId == accountId);
                if (product == null)
                {
                    err.AddError("Product not found.");
                    return default;
                }

                return _mapper.Map<ProductDto>(product);
            });
        }

        public Task<ServiceResponse<List<ProductDto>>> GetAllProducts(ProductFilterDto filter)
        {
            return ServiceResponse<List<ProductDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var query = _context.Products.Where(p => p.AccountId == accountId).AsNoTracking();

                if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                {
                    query = query.Where(p => p.ProductName!.Contains(filter.SearchTerm));
                }

                var products = await query.ToListAsync();
                return _mapper.Map<List<ProductDto>>(products);
            });
        }

        public Task<ServiceResponse<List<ProductDto>>> GetProductSuggestions(string searchTerm)
        {
            return ServiceResponse<List<ProductDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var query = _context.Products.Where(p => p.AccountId == accountId).AsNoTracking();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(p => p.ProductName!.Contains(searchTerm));
                }

                var products = await query.Take(20).ToListAsync();
                return _mapper.Map<List<ProductDto>>(products);
            });
        }

        public Task<ServiceResponse<byte[]>> ExportProducts(ProductFilterDto filter)
        {
            return ServiceResponse<byte[]>.Execute(async err =>
            {
                var accountId = Token.AccountId;
                var query = _context.Products.Where(p => p.AccountId == accountId).AsNoTracking();

                if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                {
                    query = query.Where(p => p.ProductName!.Contains(filter.SearchTerm));
                }

                var products = await query.ToListAsync();
                var dtos = _mapper.Map<List<ProductDto>>(products);
                
                // Usually we format for export, but directly exporting DTOs is fine here.
                var excelData = _excelService.Export(dtos, "Products");
                return excelData;
            });
        }

        public Task<ServiceResponse<ImportResultDto>> ImportProducts(List<ImportProductDto> dtos)
        {
            return ServiceResponse<ImportResultDto>.Execute(async err =>
            {
                var result = new ImportResultDto();
                var accountId = Token.AccountId;

                var existingNames = await _context.Products
                    .Where(p => p.AccountId == accountId)
                    .Select(p => p.ProductName)
                    .ToListAsync();

                var existingSet = new HashSet<string>(existingNames, StringComparer.OrdinalIgnoreCase);
                var toAdd = new List<Product>();

                foreach(var dto in dtos)
                {
                    dto.ProductName = dto.ProductName?.Trim();

                    if (existingSet.Contains(dto.ProductName))
                    {
                        result.Errors.Add(new ImportRowMessageDto { RowNo = dto.RowNo, Message = $"Product '{dto.ProductName}' already exists." });
                        continue;
                    }

                    var product = _mapper.Map<Product>(dto);
                    product.AccountId = accountId;
                    toAdd.Add(product);
                    
                    existingSet.Add(dto.ProductName);
                    result.SuccessCount++;
                }
                
                if (result.Errors.Count > 0)
                {
                    result.SuccessCount = 0;
                    return result;
                }
                
                await RunInTransactionAsync(async () =>
                {
                    _context.Products.AddRange(toAdd);
                });

                return result;
            });
        }
    }
}
