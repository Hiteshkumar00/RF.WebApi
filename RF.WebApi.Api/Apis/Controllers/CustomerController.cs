using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.Customer;
using RF.WebApi.Api.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            var result = await _customerService.CreateCustomer(dto);
            return HandleResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerDto dto)
        {
            var result = await _customerService.UpdateCustomer(dto);
            return HandleResponse(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerService.DeleteCustomer(id);
            return HandleResponse(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customerService.GetCustomerById(id);
            return HandleResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _customerService.GetAllCustomers();
            return HandleResponse(result);
        }
    }
}
