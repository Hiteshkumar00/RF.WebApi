namespace RF.WebApi.Api.Application.DTOs.Agency
{
    public class AgencySummaryDto
    {
        public int AgencyId { get; set; }
        public string AgencyName { get; set; } = string.Empty;
        public decimal TotalBillsAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalPendingAmount { get; set; }
    }
}
