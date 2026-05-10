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

        [HttpPost()]
        public async Task<IActionResult> GetHistory(PaymentHistoryFilterDto filter)
        {
            var result = await _paymentAccountService.GetPaymentHistory(filter);
            return HandleResponse(result);
        }

        // Payment Transfer
        [HttpPost()]
        public async Task<IActionResult> CreateTransfer(CreatePaymentTransferDto dto)
        {
            var result = await _paymentAccountService.CreatePaymentTransfer(dto);
            return HandleResponse(result);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateTransfer(UpdatePaymentTransferDto dto)
        {
            var result = await _paymentAccountService.UpdatePaymentTransfer(dto);
            return HandleResponse(result);
        }

        [HttpDelete()]
        public async Task<IActionResult> DeleteTransfer(int id)
        {
            var result = await _paymentAccountService.DeletePaymentTransfer(id);
            return HandleResponse(result);
        }

        [HttpPost()]
        public async Task<IActionResult> GetTransfers(PaymentTransferFilterDto filter)
        {
            var result = await _paymentAccountService.GetPaymentTransfers(filter);
            return HandleResponse(result);
        }
    }
}
