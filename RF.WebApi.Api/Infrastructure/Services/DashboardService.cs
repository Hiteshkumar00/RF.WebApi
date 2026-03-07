using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.Dashboard;
using RF.WebApi.Api.Application.DTOs.BusinessYear;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Infrastructure.Data.DataBase;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class DashboardService: IDashboardService
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

                var metricsResponse = await CalculateDashboardMetricsForDateRange(accountId, startDate, endDate);
                if (!metricsResponse.Success)
                {
                    err.SetErrors(metricsResponse);
                    return default;
                }

                return metricsResponse.Data;
            });
        }

        public async Task<ServiceResponse<PaymentAccountDashboardDto>> GetPaymentAccountDashboardMetricsAsync()
        {
            return await ServiceResponse<PaymentAccountDashboardDto>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                // --- PAYMENT ACCOUNT DASHBOARD (ALL TIME) ---

                // 1. Selling Bill Payments
                var sellingPayments = await _context.SellingBillPayments
                    .Where(p => p.BillId != null && _context.SellingBills.Any(b => b.Id == p.BillId && b.AccountId == accountId) && p.PaymentAccountId != null)
                    .GroupBy(p => p.PaymentAccountId)
                    .Select(g => new { PaymentAccountId = g.Key!.Value, Amount = g.Sum(p => p.Amount ?? 0) })
                    .ToListAsync();

                // 2. Buying Bill Payments
                var buyingPayments = await _context.BuyingBillPayments
                    .Where(p => p.BillId != null && _context.BuyingBills.Any(b => b.Id == p.BillId && b.AccountId == accountId) && p.PaymentAccountId != null)
                    .GroupBy(p => p.PaymentAccountId)
                    .Select(g => new { PaymentAccountId = g.Key!.Value, Amount = g.Sum(p => p.Amount ?? 0) })
                    .ToListAsync();

                // 3. Buying Bill Expences
                var buyingExpences = await _context.BuyingBillExpences
                    .Where(e => e.BillId != null && _context.BuyingBills.Any(b => b.Id == e.BillId && b.AccountId == accountId) && e.PaymentAccountId != null)
                    .GroupBy(e => e.PaymentAccountId)
                    .Select(g => new { PaymentAccountId = g.Key!.Value, Amount = g.Sum(e => e.Amount ?? 0) })
                    .ToListAsync();
                    
                // 4. Business Expences
                var businessExpences = await _context.BusinessExpencePayments
                    .Where(p => p.BusinessExpenceId != null && _context.BusinessExpences.Any(e => e.Id == p.BusinessExpenceId && e.AccountId == accountId) && p.PaymentAccountId != null)
                    .GroupBy(p => p.PaymentAccountId)
                    .Select(g => new { PaymentAccountId = g.Key!.Value, Amount = g.Sum(p => p.Amount ?? 0) })
                    .ToListAsync();

                // 5. Add Contributions
                var addContributions = await _context.AddContributionPayments
                    .Where(p => p.AddContributionId != null && _context.AddContributions.Any(c => c.Id == p.AddContributionId && _context.AccountPersons.Any(a => a.Id == c.AccountPersonId && a.AccountId == accountId)) && p.PaymentAccountId != null)
                    .GroupBy(p => p.PaymentAccountId)
                    .Select(g => new { PaymentAccountId = g.Key!.Value, Amount = g.Sum(p => p.Amount ?? 0) })
                    .ToListAsync();

                // 6. Remove Contributions
                var removeContributions = await _context.RemoveContributionPayments
                    .Where(p => p.RemoveContributionId != null && _context.RemoveContributions.Any(c => c.Id == p.RemoveContributionId && _context.AccountPersons.Any(a => a.Id == c.AccountPersonId && a.AccountId == accountId)) && p.PaymentAccountId != null)
                    .GroupBy(p => p.PaymentAccountId)
                    .Select(g => new { PaymentAccountId = g.Key!.Value, Amount = g.Sum(p => p.Amount ?? 0) })
                    .ToListAsync();

                // Fetch all payment accounts for the user to assemble the final DTOs
                var paymentAccounts = await _context.PaymentAccounts
                    .Where(pa => pa.AccountId == accountId)
                    .ToListAsync();

                var paymentAccountBalances = new List<PaymentAccountBalanceDto>();
                decimal totalAvailableBalance = 0;

                foreach (var pa in paymentAccounts)
                {
                    var id = pa.Id!.Value;

                    var selling = sellingPayments.FirstOrDefault(x => x.PaymentAccountId == id)?.Amount ?? 0;
                    var buyingPay = buyingPayments.FirstOrDefault(x => x.PaymentAccountId == id)?.Amount ?? 0;
                    var buyingExp = buyingExpences.FirstOrDefault(x => x.PaymentAccountId == id)?.Amount ?? 0;
                    var busExp = businessExpences.FirstOrDefault(x => x.PaymentAccountId == id)?.Amount ?? 0;
                    var addContrib = addContributions.FirstOrDefault(x => x.PaymentAccountId == id)?.Amount ?? 0;
                    var remContrib = removeContributions.FirstOrDefault(x => x.PaymentAccountId == id)?.Amount ?? 0;

                    // Formula: total selling - total buying payment - buying expance - business expnace + add contribution - remove contribution
                    var balance = selling - buyingPay - buyingExp - busExp + addContrib - remContrib;

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

                // 1. Fetch all Business Years Date Ranges from BusinessYearService
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
                    // Compile metrics constrained by this computed [startDate, endDate]
                    var metricsResponse = await CalculateDashboardMetricsForDateRange(accountId, dateRange.StartDate, dateRange.EndDate);
                    if (!metricsResponse.Success)
                    {
                        err.SetErrors(metricsResponse);
                        return default;
                    }
                    var metrics = metricsResponse.Data;

                    // Add mapping output for this year
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
                        BuyingPendingAmount = metrics.BuyingPendingAmount
                    });
                }

                return results;
            });
        }

        private async Task<ServiceResponse<DashboardDto>> CalculateDashboardMetricsForDateRange(int accountId, DateOnly startDate, DateOnly endDate)
        {
            return await ServiceResponse<DashboardDto>.Execute(async err =>
            {
                // 1. Total Selling
                var totalSelling = await _context.SellingBills
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .SelectMany(b => b.Items)
                    .SumAsync(item => (item.Quantity ?? 0) * (item.Price ?? 0));

                var totalSellingDiscounts = await _context.SellingBills
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .SumAsync(b => b.Discount ?? 0);
                    
                totalSelling -= totalSellingDiscounts;

                var totalSellingPaid = await _context.SellingBills
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .SelectMany(b => b.Payments)
                    .SumAsync(p => p.Amount ?? 0);

                // 2. Total Buying
                var totalBuyingItems = await _context.BuyingBills
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .SelectMany(b => b.Items)
                    .SumAsync(item => (item.Quantity ?? 0) * (item.Price ?? 0));

                var totalBuyingExpence = await _context.BuyingBills
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .SelectMany(b => b.Expences)
                    .SumAsync(e => e.Amount ?? 0);
                    
                var totalBuyingDiscounts = await _context.BuyingBills
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .SumAsync(b => b.Discount ?? 0);

                var totalBuying = totalBuyingItems + totalBuyingExpence - totalBuyingDiscounts;

                var totalBuyingPaid = await _context.BuyingBills
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .SelectMany(b => b.Payments)
                    .SumAsync(p => p.Amount ?? 0);

                // 3. Total Business Expence
                var totalBusinessExpencePayments = await _context.BusinessExpences
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .SelectMany(b => b.Payments)
                    .SumAsync(p => p.Amount ?? 0);

                // Calculations
                var totalProfit = totalSelling - totalBuying - totalBusinessExpencePayments;
                var sellingPending = totalSelling - totalSellingPaid;
                var buyingPending = totalBuying - totalBuyingPaid;

                return new DashboardDto
                {
                    TotalSellingAmount = totalSelling,
                    TotalBuyingAmount = totalBuying,
                    TotalExpenceAmount = totalBusinessExpencePayments,
                    TotalProfit = totalProfit,
                    SellingPendingAmount = sellingPending,
                    BuyingPendingAmount = buyingPending
                };
            });
        }
    }
}
