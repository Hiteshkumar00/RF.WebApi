using System.Collections.Generic;

namespace RF.WebApi.Api.Application.DTOs.Agency
{
    public class AgencyAdvancedListDto : AgencyDto
    {
        public decimal TotalBillsAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalPendingAmount { get; set; }
    }
}
