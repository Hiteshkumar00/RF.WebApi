using System.Collections.Generic;
using System.Threading.Tasks;
using RF.WebApi.Api.Application.DTOs.Customer;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface ICustomerService
    {
        Task<ServiceResponse<int>> CreateCustomer(CreateCustomerDto dto);
        Task<ServiceResponse<bool>> UpdateCustomer(UpdateCustomerDto dto);
        Task<ServiceResponse<bool>> DeleteCustomer(int id);
        Task<ServiceResponse<CustomerDto>> GetCustomerById(int id);
        Task<ServiceResponse<List<CustomerListDto>>> GetAllCustomers();
    }
}
