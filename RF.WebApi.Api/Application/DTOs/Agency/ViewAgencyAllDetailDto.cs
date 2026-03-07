using RF.WebApi.Api.Application.DTOs.AgencyPerson;
using RF.WebApi.Api.Application.DTOs.BuyingBill;
using System.Collections.Generic;

namespace RF.WebApi.Api.Application.DTOs.Agency
{
    public class ViewAgencyAllDetailDto : AgencyDto
    {
        public decimal TotalBillsAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalPendingAmount { get; set; }
        
        public List<AgencyPersonDto> AgencyPersons { get; set; } = new();
        public List<AgencyBillsByYearDto> BillsByYear { get; set; } = new();
    }

    public class AgencyBillsByYearDto
    {
        public int? BusinessYearId { get; set; }
        public string YearName { get; set; } = string.Empty;
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public List<BuyingBillListDto> Bills { get; set; } = new();
    }
}
