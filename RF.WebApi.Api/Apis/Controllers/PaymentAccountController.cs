using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.PaymentAccount;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentAccountController : BaseController
    {
        private readonly IPaymentAccountService _paymentAccountService;

        public PaymentAccountController(IPaymentAccountService paymentAccountService)
        {
            _paymentAccountService = paymentAccountService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreatePaymentAccountDto dto)
        {
            var result = await _paymentAccountService.CreatePaymentAccount(dto);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _paymentAccountService.GetPaymentAccountById(id);
            return HandleResponse(result);
        }

        [HttpPut()]
        public async Task<IActionResult> Update(UpdatePaymentAccountDto dto)
        {
            var result = await _paymentAccountService.UpdatePaymentAccount(dto);
            return HandleResponse(result);
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _paymentAccountService.DeletePaymentAccount(id);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _paymentAccountService.GetAllPaymentAccounts();
            return HandleResponse(result);
        }
    }
}
