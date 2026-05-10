namespace RF.WebApi.Api.Application.DTOs.Dashboard
{
    public class ProductProfitDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageLink { get; set; }
        public int TotalSoldCount { get; set; }
        public int TotalPurchaseCount { get; set; }
        public decimal TotalSellingAmount { get; set; }
        public decimal TotalPurchaseCost { get; set; }
        public decimal TotalProfit { get; set; }
        public int AvailableStock { get; set; }
        public List<ProductStockHistoryDto> StockHistory { get; set; } = new();
    }

    public class ProductStockHistoryDto
    {
        public int StockId { get; set; }
        public string BillNo { get; set; } = string.Empty;
        public string AgencyName { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Discount { get; set; }
        public int RemainingQty { get; set; }
        public decimal TotalAmount => (Quantity * PurchasePrice) - (Quantity * Discount);
    }

    public class ProductDashboardDto
    {
        public List<ProductProfitDto> ProductProfits { get; set; } = new();
    }
}
