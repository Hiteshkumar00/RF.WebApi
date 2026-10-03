namespace RF.WebApi.Api.Application.DTOs.SellingBill
{
    public class SellingBillStatisticsDto
    {
        public decimal TotalSellingAmount { get; set; }
        public decimal TotalReceivedAmount { get; set; }
        public decimal TotalRemainingAmount { get; set; }
    }
}
