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
        public decimal TotalBuyingAmount { get; set; }
        public decimal TotalPurchaseCost { get; set; }
        public decimal TotalProfit { get; set; }
        public int AvailableStock { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public List<ProductStockHistoryDto> StockHistory { get; set; } = new();
    }

    public class ProductStockHistoryDto
    {
        public int StockId { get; set; }
        public int BillId { get; set; }
        public string BillNo { get; set; } = string.Empty;
        public string AgencyName { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPerItem => Quantity > 0 ? Discount / Quantity : 0;
        public decimal NetPurchasePrice => PurchasePrice - DiscountPerItem;
        public decimal TotalNetPurchasedAmount => NetPurchasePrice * Quantity;
        public int RemainingQty { get; set; }
        public int TotalSold => Quantity - RemainingQty;
    }

    public class ProductDashboardDto
    {
        public List<ProductProfitDto> ProductProfits { get; set; } = new();
    }
}
