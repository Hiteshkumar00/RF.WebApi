using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.SellingBill;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SellingBillController : BaseController
    {
        private readonly ISellingBillService _sellingBillService;

        public SellingBillController(ISellingBillService sellingBillService)
        {
            _sellingBillService = sellingBillService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSellingBillDto dto)
        {
            var result = await _sellingBillService.CreateSellingBill(dto);
            return HandleResponse(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _sellingBillService.GetSellingBillById(id);
            return HandleResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSellingBillDto dto)
        {
            var result = await _sellingBillService.UpdateSellingBill(dto);
            return HandleResponse(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sellingBillService.DeleteSellingBill(id);
            return HandleResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sellingBillService.GetAllSellingBills();
            return HandleResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetItemSuggestions()
        {
            var result = await _sellingBillService.GetItemSuggestions();
            return HandleResponse(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadInvoice(int id)
        {
            var result = await _sellingBillService.GenerateInvoicePdf(id);
            if (result.Success)
            {
                return File(result.Data, "application/pdf", $"Selling_Bill_{id}.pdf");
            }
            return HandleResponse(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> PrintInvoice(int id)
        {
            var result = await _sellingBillService.GenerateInvoicePdf(id);
            if (result.Success)
            {
                return File(result.Data, "application/pdf");
            }
            return HandleResponse(result);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> SendWhatsAppMessage(int id)
        {
            var result = await _sellingBillService.SendWhatsAppMessage(id);
            return HandleResponse(result);
        }
    }
}
