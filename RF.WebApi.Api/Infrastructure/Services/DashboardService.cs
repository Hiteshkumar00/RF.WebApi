using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.Dashboard;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly RFDBContext _context;
        private readonly IBusinessYearService _businessYearService;

        public DashboardService(RFDBContext context, IBusinessYearService businessYearService)
        {
            _context = context;
            _businessYearService = businessYearService;
        }

        public async Task<ServiceResponse<DashboardDto>> GetDashboardMetricsAsync()
        {
            return await ServiceResponse<DashboardDto>.Execute(async err =>
            {
                var accountId = Token.AccountId;
                var dateRangeResponse = await _businessYearService.GetSelectedBusinessYearDates();
                if (!dateRangeResponse.Success)
                {
                    err.SetErrors(dateRangeResponse);
                    return default;
                }

                var (startDate, endDate) = dateRangeResponse.Data;
                var result = await CalculateDashboardMetricsForDateRange(accountId, startDate, endDate);
                return result.Data;
            });
        }

        public async Task<ServiceResponse<PaymentAccountDashboardDto>> GetPaymentAccountDashboardMetricsAsync()
        {
            return await ServiceResponse<PaymentAccountDashboardDto>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                // 1. Selling Payments (money IN)
                var sellingPayments = await _context.SellingBillPayments
                    .Where(p => _context.SellingBills.Any(b => b.Id == p.BillId && b.AccountId == accountId) && p.PaymentAccountId != null)
                    .GroupBy(p => p.PaymentAccountId)
                    .Select(g => new { PaymentAccountId = g.Key!.Value, Amount = g.Sum(p => p.Amount ?? 0) })
                    .ToListAsync();

                // 2. Buying Payments (money OUT)
                var buyingPayments = await _context.BuyingBillPayments
                    .Where(p => _context.BuyingBills.Any(b => b.Id == p.BillId && b.AccountId == accountId) && p.PaymentAccountId != null)
                    .GroupBy(p => p.PaymentAccountId)
                    .Select(g => new { PaymentAccountId = g.Key!.Value, Amount = g.Sum(p => p.Amount ?? 0) })
                    .ToListAsync();

                // 3. Business Expences (money OUT)
                var businessExpences = await _context.BusinessExpencePayments
                    .Where(p => _context.BusinessExpences.Any(e => e.Id == p.BusinessExpenceId && e.AccountId == accountId) && p.PaymentAccountId != null)
                    .GroupBy(p => p.PaymentAccountId)
                    .Select(g => new { PaymentAccountId = g.Key!.Value, Amount = g.Sum(p => p.Amount ?? 0) })
                    .ToListAsync();

                // 4. Add Contributions (money IN)
                var addContributions = await _context.AddContributionPayments
                    .Where(p => p.AddContributionId != null && _context.AddContributions.Any(c => c.Id == p.AddContributionId && c.AccountId == accountId) && p.PaymentAccountId != null)
                    .GroupBy(p => p.PaymentAccountId)
                    .Select(g => new { PaymentAccountId = g.Key!.Value, Amount = g.Sum(p => p.Amount ?? 0) })
                    .ToListAsync();

                // 5. Remove Contributions (money OUT)
                var removeContributions = await _context.RemoveContributionPayments
                    .Where(p => p.RemoveContributionId != null && _context.RemoveContributions.Any(c => c.Id == p.RemoveContributionId && c.AccountId == accountId) && p.PaymentAccountId != null)
                    .GroupBy(p => p.PaymentAccountId)
                    .Select(g => new { PaymentAccountId = g.Key!.Value, Amount = g.Sum(p => p.Amount ?? 0) })
                    .ToListAsync();

                // 6. Transfers OUT (money OUT)
                var transfersOut = await _context.PaymentTransfers
                    .Where(t => t.AccountId == accountId && t.FromPaymentAccountId != null)
                    .GroupBy(t => t.FromPaymentAccountId)
                    .Select(g => new { PaymentAccountId = g.Key!.Value, Amount = g.Sum(t => t.Amount ?? 0) })
                    .ToListAsync();

                // 7. Transfers IN (money IN)
                var transfersIn = await _context.PaymentTransfers
                    .Where(t => t.AccountId == accountId && t.ToPaymentAccountId != null)
                    .GroupBy(t => t.ToPaymentAccountId)
                    .Select(g => new { PaymentAccountId = g.Key!.Value, Amount = g.Sum(t => t.Amount ?? 0) })
                    .ToListAsync();

                var paymentAccounts = await _context.PaymentAccounts
                    .Where(pa => pa.AccountId == accountId)
                    .ToListAsync();

                var paymentAccountBalances = new List<PaymentAccountBalanceDto>();
                decimal totalAvailableBalance = 0;

                foreach (var pa in paymentAccounts)
                {
                    var id = pa.Id!.Value;

                    var selling   = sellingPayments.FirstOrDefault(x => x.PaymentAccountId == id)?.Amount ?? 0;
                    var buyingPay = buyingPayments.FirstOrDefault(x => x.PaymentAccountId == id)?.Amount ?? 0;
                    var busExp    = businessExpences.FirstOrDefault(x => x.PaymentAccountId == id)?.Amount ?? 0;
                    var addCont   = addContributions.FirstOrDefault(x => x.PaymentAccountId == id)?.Amount ?? 0;
                    var remCont   = removeContributions.FirstOrDefault(x => x.PaymentAccountId == id)?.Amount ?? 0;
                    var transOut  = transfersOut.FirstOrDefault(x => x.PaymentAccountId == id)?.Amount ?? 0;
                    var transIn   = transfersIn.FirstOrDefault(x => x.PaymentAccountId == id)?.Amount ?? 0;

                    // Formula: selling + addCont + transIn - buyingPay - busExp - remCont - transOut
                    var balance = selling + addCont + transIn - buyingPay - busExp - remCont - transOut;

                    paymentAccountBalances.Add(new PaymentAccountBalanceDto
                    {
                        PaymentAccountId = id,
                        PaymentAccountName = pa.MethodName ?? "Unknown",
                        Balance = balance
                    });

                    totalAvailableBalance += balance;
                }

                return new PaymentAccountDashboardDto
                {
                    TotalAvailableBalance = totalAvailableBalance,
                    PaymentAccountBalances = paymentAccountBalances
                };
            });
        }

        public async Task<ServiceResponse<List<AllTimeDashboardItemDto>>> GetAllTimeDashboardMetricsAsync()
        {
            return await ServiceResponse<List<AllTimeDashboardItemDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var dateRangesResponse = await _businessYearService.GetAllBusinessYearDateRanges();
                if (!dateRangesResponse.Success)
                {
                    err.SetErrors(dateRangesResponse);
                    return default;
                }

                var results = new List<AllTimeDashboardItemDto>();

                if (dateRangesResponse.Data == null)
                {
                    return results;
                }

                foreach (var dateRange in dateRangesResponse.Data)
                {
                    var metricsResponse = await CalculateDashboardMetricsForDateRange(accountId, dateRange.StartDate, dateRange.EndDate);
                    if (!metricsResponse.Success)
                    {
                        err.SetErrors(metricsResponse);
                        return default;
                    }
                    var metrics = metricsResponse.Data;

                    results.Add(new AllTimeDashboardItemDto
                    {
                        BusinessYearId = dateRange.Id,
                        BusinessYearName = dateRange.YearName,
                        StartDate = dateRange.StartDate,
                        EndDate = dateRange.EndDate,
                        TotalSellingAmount = metrics.TotalSellingAmount,
                        TotalBuyingAmount = metrics.TotalBuyingAmount,
                        TotalExpenceAmount = metrics.TotalExpenceAmount,
                        TotalProfit = metrics.TotalProfit,
                        SellingPendingAmount = metrics.SellingPendingAmount,
                        BuyingPendingAmount = metrics.BuyingPendingAmount,
                        ExpencePendingAmount = metrics.ExpencePendingAmount
                    });
                }

                return results;
            });
        }

        private async Task<ServiceResponse<DashboardDto>> CalculateDashboardMetricsForDateRange(int accountId, DateOnly startDate, DateOnly endDate)
        {
            return await ServiceResponse<DashboardDto>.Execute(async err =>
            {
                // 1. Total Selling (net of discount)
                var totalSelling = await _context.SellingBills
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .SelectMany(b => b.Items)
                    .SumAsync(item => (item.Quantity ?? 0) * (item.Price ?? 0));

                var totalSellingDiscounts = await _context.SellingBills
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .SelectMany(b => b.Items)
                    .SumAsync(i => (i.Quantity ?? 0) * (i.Discount ?? 0));

                totalSelling -= totalSellingDiscounts;

                var totalSellingPaid = await _context.SellingBills
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .SelectMany(b => b.Payments)
                    .SumAsync(p => p.Amount ?? 0);

                // 2. Total Buying (Purchases made in this period)
                var totalBuyingItems = await _context.BuyingBills
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .SelectMany(b => b.Stocks)
                    .SumAsync(item => (item.Quantity ?? 0) * (item.PurchasePrice ?? 0));

                var totalBuyingDiscounts = await _context.BuyingBills
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .SelectMany(b => b.Stocks)
                    .SumAsync(i => (i.Quantity ?? 0) * (i.Discount ?? 0));

                var totalBuying = totalBuyingItems - totalBuyingDiscounts;

                var totalBuyingPaid = await _context.BuyingBills
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .SelectMany(b => b.Payments)
                    .SumAsync(p => p.Amount ?? 0);

                // 3. ALL Business Expences
                var totalBusinessExpenceDeclared = await _context.BusinessExpences
                    .Where(e => e.AccountId == accountId && e.Date >= startDate && e.Date <= endDate)
                    .SumAsync(e => e.TotalAmount ?? 0);

                var totalBusinessExpencePaid = await _context.BusinessExpences
                    .Where(e => e.AccountId == accountId && e.Date >= startDate && e.Date <= endDate)
                    .SelectMany(e => e.Payments)
                    .SumAsync(p => p.Amount ?? 0);

                // 4. Profit Calculation (Gross Profit from Sales only)
                decimal totalCogs = 0;
                var sellingItems = await _context.SellingBillItems
                    .Where(i => i.Bill!.AccountId == accountId && i.Bill.Date >= startDate && i.Bill.Date <= endDate)
                    .ToListAsync();
                
                var productGroups = sellingItems.GroupBy(i => i.ProductId);

                foreach (var group in productGroups)
                {
                    var productId = group.Key!.Value;
                    var qtySoldInPeriod = group.Sum(i => i.Quantity ?? 0);

                    // FIFO Logic for COGS
                    var stocks = await _context.Stocks
                        .Where(s => s.ProductId == productId && s.Date <= endDate)
                        .OrderBy(s => s.Date)
                        .ToListAsync();

                    var soldBefore = await _context.SellingBillItems
                        .Where(sbi => sbi.ProductId == productId && sbi.Bill!.AccountId == accountId && sbi.Bill.Date < startDate)
                        .SumAsync(s => s.Quantity ?? 0);

                    int skip = soldBefore;
                    int cost = qtySoldInPeriod;

                    foreach (var stock in stocks)
                    {
                        if (cost <= 0) break;
                        int stockQty = stock.Quantity ?? 0;
                        if (stockQty <= 0) continue;

                        if (skip >= stockQty) { skip -= stockQty; continue; }

                        int avail = stockQty - skip;
                        skip = 0;
                        int consume = Math.Min(avail, cost);
                        totalCogs += consume * ((stock.PurchasePrice ?? 0) - (stock.Discount ?? 0));
                        cost -= consume;
                    }

                    if (cost > 0)
                    {
                        var last = stocks.LastOrDefault();
                        totalCogs += cost * (last != null ? (last.PurchasePrice ?? 0) - (last.Discount ?? 0) : 0);
                    }
                }

                var totalProfit = totalSelling - totalCogs;

                return new DashboardDto
                {
                    TotalSellingAmount = totalSelling,
                    TotalBuyingAmount = totalBuying,
                    TotalExpenceAmount = totalBusinessExpenceDeclared,
                    TotalProfit = totalProfit,
                    SellingPendingAmount = totalSelling - totalSellingPaid,
                    BuyingPendingAmount = totalBuying - totalBuyingPaid,
                    ExpencePendingAmount = totalBusinessExpenceDeclared - totalBusinessExpencePaid
                };
            });
        }

        public async Task<ServiceResponse<ProductDashboardDto>> GetProductProfitMetricsAsync()
        {
            return await ServiceResponse<ProductDashboardDto>.Execute(async err =>
            {
                var accountId = Token.AccountId;
                var products = await _context.Products.Where(p => p.AccountId == accountId).ToListAsync();
                var productProfits = new List<ProductProfitDto>();

                foreach (var product in products)
                {
                    var productId = product.Id!.Value;

                    var stocks = await _context.Stocks
                        .Include(s => s.BuyingBill).ThenInclude(b => b.Agency)
                        .Where(s => s.ProductId == productId)
                        .OrderBy(s => s.Date).ToListAsync();

                    var sales = await _context.SellingBillItems.Where(sbi => sbi.ProductId == productId).ToListAsync();

                    var totalSold = sales.Sum(s => s.Quantity ?? 0);
                    var totalBought = stocks.Sum(s => s.Quantity ?? 0);
                    var totalSellingAmt = sales.Sum(s => (s.Quantity ?? 0) * ((s.Price ?? 0) - (s.Discount ?? 0)));
                    
                    // FIFO Remaining
                    int soldSoFar = totalSold;
                    var history = new List<ProductStockHistoryDto>();
                    foreach (var s in stocks)
                    {
                        int remain = soldSoFar >= (s.Quantity ?? 0) ? 0 : (s.Quantity ?? 0) - soldSoFar;
                        soldSoFar = Math.Max(0, soldSoFar - (s.Quantity ?? 0));

                        history.Add(new ProductStockHistoryDto {
                            StockId = s.Id!.Value,
                            BillNo = s.BuyingBill?.BillNo ?? "N/A",
                            AgencyName = s.BuyingBill?.Agency?.AgencyName ?? "Direct Stock",
                            Date = s.Date?.ToDateTime(TimeOnly.MinValue),
                            Quantity = s.Quantity ?? 0,
                            PurchasePrice = s.PurchasePrice ?? 0,
                            Discount = s.Discount ?? 0,
                            RemainingQty = remain
                        });
                    }

                    // COGS (All Time)
                    decimal cogs = 0;
                    int costLeft = totalSold;
                    foreach (var s in stocks)
                    {
                        if (costLeft <= 0) break;
                        int consume = Math.Min(s.Quantity ?? 0, costLeft);
                        cogs += consume * ((s.PurchasePrice ?? 0) - (s.Discount ?? 0));
                        costLeft -= consume;
                    }
                    if (costLeft > 0) {
                        var last = stocks.LastOrDefault();
                        cogs += costLeft * (last != null ? (last.PurchasePrice ?? 0) - (last.Discount ?? 0) : 0);
                    }

                    productProfits.Add(new ProductProfitDto {
                        ProductId = productId,
                        ProductName = product.ProductName ?? "Unknown",
                        ImageLink = product.ImageLink,
                        TotalSoldCount = totalSold,
                        TotalPurchaseCount = totalBought,
                        TotalSellingAmount = totalSellingAmt,
                        TotalPurchaseCost = cogs,
                        TotalProfit = totalSellingAmt - cogs,
                        AvailableStock = totalBought - totalSold,
                        StockHistory = history.OrderByDescending(h => h.Date).ToList()
                    });
                }

                return new ProductDashboardDto { ProductProfits = productProfits.OrderByDescending(p => p.TotalProfit).ToList() };
            });
        }
    }
}
