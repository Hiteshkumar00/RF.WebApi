using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.Customer;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly RFDBContext _context;
        private readonly IMapper _mapper;

        public CustomerService(RFDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<int>> CreateCustomer(CreateCustomerDto dto)
        {
            return await ServiceResponse<int>.Execute(async err =>
            {
                var customer = _mapper.Map<Customer>(dto);
                customer.AccountId = Token.AccountId;

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return customer.Id ?? 0;
            });
        }

        public Task<ServiceResponse<bool>> UpdateCustomer(UpdateCustomerDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Id == dto.Id && c.AccountId == Token.AccountId);

                if (customer == null)
                {
                    err.AddError("Customer not found.");
                    return false;
                }

                _mapper.Map(dto, customer);
                await _context.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<bool>> DeleteCustomer(int id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var customer = await _context.Customers
                    .Include(c => c.SellingBills)
                    .FirstOrDefaultAsync(c => c.Id == id && c.AccountId == Token.AccountId);

                if (customer == null)
                {
                    err.AddError("Customer not found.");
                    return false;
                }

                if (customer.SellingBills.Any())
                {
                    err.AddError("Cannot delete customer with existing selling bills.");
                    return false;
                }

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<CustomerDto>> GetCustomerById(int id)
        {
            return ServiceResponse<CustomerDto>.Execute(async err =>
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Id == id && c.AccountId == Token.AccountId);

                if (customer == null)
                {
                    err.AddError("Customer not found.");
                    return default;
                }

                return _mapper.Map<CustomerDto>(customer);
            });
        }

        public Task<ServiceResponse<List<CustomerListDto>>> GetAllCustomers()
        {
            return ServiceResponse<List<CustomerListDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;
                var customers = await _context.Customers
                    .Where(c => c.AccountId == accountId)
                    .Select(c => new CustomerListDto
                    {
                        Id = c.Id ?? 0,
                        CustomerName = c.CustomerName,
                        PhoneNo = c.PhoneNo,
                        Email = c.Email,
                        Address = c.Address,
                        TotalPurchases = c.SellingBills.Count,
                        TotalNetAmount = c.SellingBills
                            .SelectMany(sb => sb.Items)
                            .Sum(i => (decimal?)((i.Quantity ?? 0) * (i.Price ?? 0) - (i.Discount ?? 0))) ?? 0,
                        TotalRemainingAmount = (c.SellingBills
                            .SelectMany(sb => sb.Items)
                            .Sum(i => (decimal?)((i.Quantity ?? 0) * (i.Price ?? 0) - (i.Discount ?? 0))) ?? 0)
                            - (c.SellingBills
                            .SelectMany(sb => sb.Payments)
                            .Sum(p => (decimal?)p.Amount) ?? 0)
                    })
                    .OrderBy(c => c.CustomerName)
                    .ToListAsync();

                return customers;
            });
        }
    }
}
