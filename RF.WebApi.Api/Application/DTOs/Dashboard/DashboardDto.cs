namespace RF.WebApi.Api.Application.DTOs.Dashboard
{
    public class DashboardDto
    {
        public decimal TotalSellingAmount { get; set; }
        public decimal TotalBuyingAmount { get; set; }
        public decimal TotalExpenceAmount { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal SellingPendingAmount { get; set; }
        public decimal BuyingPendingAmount { get; set; }
        public decimal ExpencePendingAmount { get; set; }
    }
}
