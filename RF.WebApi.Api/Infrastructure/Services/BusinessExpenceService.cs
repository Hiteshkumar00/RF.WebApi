using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.BusinessExpence;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;


namespace RF.WebApi.Api.Infrastructure.Services
{
    public class BusinessExpenceService : IBusinessExpenceService
    {
        private readonly RFDBContext _context;
        private readonly IMapper _mapper;
        private readonly IBusinessYearService _businessYearService;

        public BusinessExpenceService(RFDBContext context, IMapper mapper, IBusinessYearService businessYearService)
        {
            _context = context;
            _mapper = mapper;
            _businessYearService = businessYearService;
        }

        public async Task<ServiceResponse<int>> CreateBusinessExpence(CreateBusinessExpenceDto dto)
        {
            return await ServiceResponse<int>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var expense = _mapper.Map<BusinessExpence>(dto);
                expense.AccountId = accountId;

                _context.BusinessExpences.Add(expense);
                
                foreach (var payment in expense.Payments)
                {
                    if (payment.Date == null) payment.Date = expense.Date;
                }

                await _context.SaveChangesAsync();

                return expense.Id ?? default;
            });
        }

        public Task<ServiceResponse<BusinessExpenceDto>> GetBusinessExpenceById(int id)
        {
            return ServiceResponse<BusinessExpenceDto>.Execute(async err =>
            {
                var expense = await _context.BusinessExpences
                    .Include(e => e.Payments)
                    .Include(e => e.BuyingBill)
                        .ThenInclude(b => b != null ? b.Agency : null)
                    .FirstOrDefaultAsync(e => e.Id == id && e.AccountId == Token.AccountId);

                if (expense == null)
                {
                    err.AddError(BusinessExpenceMessages.NotFound);
                    return default;
                }

                var dto = _mapper.Map<BusinessExpenceDto>(expense);
                dto.AgencyName = expense.BuyingBill?.Agency?.AgencyName;

                // Resolve payment account names
                var accountIds = expense.Payments
                    .Where(p => p.PaymentAccountId.HasValue)
                    .Select(p => p.PaymentAccountId!.Value)
                    .Distinct()
                    .ToList();

                var accounts = await _context.PaymentAccounts
                    .Where(pa => accountIds.Contains(pa.Id!.Value))
                    .ToDictionaryAsync(pa => pa.Id!.Value, pa => pa.MethodName ?? string.Empty);

                foreach (var payment in dto.Payments)
                {
                    payment.PaymentAccountName = accounts.TryGetValue(payment.PaymentAccountId, out var name) ? name : string.Empty;
                }

                return dto;
            });
        }

        public Task<ServiceResponse<bool>> UpdateBusinessExpence(UpdateBusinessExpenceDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var expense = await _context.BusinessExpences
                    .Include(e => e.Payments)
                    .FirstOrDefaultAsync(e => e.Id == dto.Id && e.AccountId == Token.AccountId);

                if (expense == null)
                {
                    err.AddError(BusinessExpenceMessages.NotFound);
                    return false;
                }

                // Sync scalar properties (Payments and BuyingBillId are preserved as-is)
                _mapper.Map(dto, expense);

                // Generic helper handles Add, Update, and Remove for collection safely
                _context.SyncCollection(expense.Payments, dto.Payments, (e, d) => d.Id > 0 && e.Id == d.Id, _mapper);

                foreach (var payment in expense.Payments)
                {
                    if (payment.Date == null) payment.Date = expense.Date;
                }

                await _context.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<bool>> DeleteBusinessExpence(int id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var expense = await _context.BusinessExpences
                    .Include(e => e.Payments)
                    .FirstOrDefaultAsync(e => e.Id == id && e.AccountId == Token.AccountId);

                if (expense == null)
                {
                    err.AddError(BusinessExpenceMessages.NotFound);
                    return false;
                }

                _context.BusinessExpencePayments.RemoveRange(expense.Payments);
                _context.BusinessExpences.Remove(expense);

                await _context.SaveChangesAsync();

                return true;
            });
        }

        public Task<ServiceResponse<List<BusinessExpenceListDto>>> GetAllBusinessExpences()
        {
            return ServiceResponse<List<BusinessExpenceListDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                // 1. Determine Date Range from the User's selected Business Year
                var dateRangeResponse = await _businessYearService.GetSelectedBusinessYearDates();
                if (!dateRangeResponse.Success)
                {
                    err.SetErrors(dateRangeResponse);
                    return default;
                }

                var (startDate, endDate) = dateRangeResponse.Data;

                // 2. Fetch all expenses (both direct and buying-bill-linked) with Agency info
                var expenses = await _context.BusinessExpences
                    .Include(e => e.Payments)
                    .Include(e => e.BuyingBill)
                        .ThenInclude(b => b != null ? b.Agency : null)
                    .Where(e => e.AccountId == accountId && e.Date >= startDate && e.Date <= endDate)
                    .OrderByDescending(e => e.Date)
                    .AsNoTracking()
                    .ToListAsync();

                // 3. Map and resolve AgencyName manually
                var result = expenses.Select(e =>
                {
                    var dto = _mapper.Map<BusinessExpenceListDto>(e);
                    dto.AgencyName = e.BuyingBill?.Agency?.AgencyName;
                    return dto;
                }).ToList();

                return result;
            });
        }

        public Task<ServiceResponse<List<string>>> GetExpenceTypeSuggestions()
        {
            return ServiceResponse<List<string>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var suggestions = await _context.BusinessExpences
                    .Where(e => e.AccountId == accountId && !string.IsNullOrWhiteSpace(e.ExpenceType))
                    .Select(e => e.ExpenceType!)
                    .GroupBy(x => x.Trim())
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .ToListAsync();

                return suggestions;
            });
        }
    }
}
