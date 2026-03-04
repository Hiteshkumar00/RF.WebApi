using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.BuyingBill;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BuyingBillController : BaseController
    {
        private readonly IBuyingBillService _buyingBillService;

        public BuyingBillController(IBuyingBillService buyingBillService)
        {
            _buyingBillService = buyingBillService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreateBuyingBillDto dto)
        {
            var result = await _buyingBillService.CreateBuyingBill(dto);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _buyingBillService.GetBuyingBillById(id);
            return HandleResponse(result);
        }

        [HttpPut()]
        public async Task<IActionResult> Update(UpdateBuyingBillDto dto)
        {
            var result = await _buyingBillService.UpdateBuyingBill(dto);
            return HandleResponse(result);
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _buyingBillService.DeleteBuyingBill(id);
            return HandleResponse(result);
        }

        /// <summary>
        /// Retrieves all Buying Bills filtered by the current Business Year dates.
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _buyingBillService.GetAllBuyingBills();
            return HandleResponse(result);
        }

        /// <summary>
        /// Retrieves all Buying Bills for a specific Agency, filtered by the current Business Year dates.
        /// </summary>
        [HttpGet("{agencyId}")]
        public async Task<IActionResult> GetAllByAgencyId(int agencyId)
        {
            var result = await _buyingBillService.GetAllBuyingBillsByAgencyId(agencyId);
            return HandleResponse(result);
        }
    }
}
