using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.AgencyPayment;
using RF.WebApi.Api.Domain.Interfaces;
using System.Threading.Tasks;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AgencyPaymentController : BaseController
    {
        private readonly IAgencyPaymentService _agencyPaymentService;

        public AgencyPaymentController(IAgencyPaymentService agencyPaymentService)
        {
            _agencyPaymentService = agencyPaymentService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreateAgencyPaymentDto dto)
        {
            var result = await _agencyPaymentService.CreateAgencyPayment(dto);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _agencyPaymentService.GetAgencyPaymentById(id);
            return HandleResponse(result);
        }

        [HttpPut()]
        public async Task<IActionResult> Update(UpdateAgencyPaymentDto dto)
        {
            var result = await _agencyPaymentService.UpdateAgencyPayment(dto);
            return HandleResponse(result);
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _agencyPaymentService.DeleteAgencyPayment(id);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _agencyPaymentService.GetAllAgencyPayments();
            return HandleResponse(result);
        }
    }
}
