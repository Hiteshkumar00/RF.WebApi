using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.BuyingBill;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;


namespace RF.WebApi.Api.Infrastructure.Services
{
    public class BuyingBillService : IBuyingBillService
    {
        private readonly RFDBContext _context;
        private readonly IMapper _mapper;
        private readonly IBusinessYearService _businessYearService;

        public BuyingBillService(RFDBContext context, IMapper mapper, IBusinessYearService businessYearService)
        {
            _context = context;
            _mapper = mapper;
            _businessYearService = businessYearService;
        }

        public async Task<ServiceResponse<int>> CreateBuyingBill(CreateBuyingBillDto dto)
        {
            return await ServiceResponse<int>.Execute(async err =>
            {
                var bill = _mapper.Map<BuyingBill>(dto);
                bill.AccountId = Token.AccountId;
                bill.BillNo = "TEMP_BILL_NO"; // Temporary value to bypass NOT NULL constraint during insert

                _context.BuyingBills.Add(bill);
                await _context.SaveChangesAsync();

                if (bill.BillNo == "TEMP_BILL_NO")
                {
                    var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == Token.AccountId);
                    string initials = "ABC";
                    if (account != null && !string.IsNullOrWhiteSpace(account.ProfileName))
                    {
                        var words = account.ProfileName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (words.Length > 0)
                        {
                            initials = string.Join("", words.Select(w => w[0])).ToUpper();
                        }
                    }
                    
                    string year = bill.Date.HasValue ? bill.Date.Value.Year.ToString() : DateTime.Now.Year.ToString();
                    bill.BillNo = $"{initials}{year}BB{bill.Id}";
                    await _context.SaveChangesAsync();
                }

                return bill.Id ?? default;
            });
        }

        public Task<ServiceResponse<BuyingBillDto>> GetBuyingBillById(int id)
        {
            return ServiceResponse<BuyingBillDto>.Execute(async err =>
            {
                var bill = await _context.BuyingBills
                    .Include(b => b.Items)
                    .Include(b => b.Payments)
                    .Include(b => b.Expences)
                    .FirstOrDefaultAsync(b => b.Id == id && b.AccountId == Token.AccountId);

                if (bill == null)
                {
                    err.AddError(BuyingBillMessages.NotFound);
                    return default;
                }

                return _mapper.Map<BuyingBillDto>(bill);
            });
        }

        public Task<ServiceResponse<bool>> UpdateBuyingBill(UpdateBuyingBillDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var bill = await _context.BuyingBills
                    .Include(b => b.Items)
                    .Include(b => b.Payments)
                    .Include(b => b.Expences)
                    .FirstOrDefaultAsync(b => b.Id == dto.Id && b.AccountId == Token.AccountId);

                if (bill == null)
                {
                    err.AddError(BuyingBillMessages.NotFound);
                    return false;
                }

                // Sync scalar properties (Items, Payments, Expences are ignored in Profile)
                _mapper.Map(dto, bill);
                
                // Generic helper handles Add, Update, and Remove for collections safely
                _context.SyncCollection(bill.Items, dto.Items, (e, d) => d.Id > 0 && e.Id == d.Id, _mapper);
                _context.SyncCollection(bill.Payments, dto.Payments, (e, d) => d.Id > 0 && e.Id == d.Id, _mapper);
                _context.SyncCollection(bill.Expences, dto.Expences, (e, d) => d.Id > 0 && e.Id == d.Id, _mapper);

                await _context.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<bool>> DeleteBuyingBill(int id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var bill = await _context.BuyingBills
                    .Include(b => b.Items)
                    .Include(b => b.Payments)
                    .Include(b => b.Expences)
                    .FirstOrDefaultAsync(b => b.Id == id && b.AccountId == Token.AccountId);

                if (bill == null)
                {
                    err.AddError(BuyingBillMessages.NotFound);
                    return false;
                }

                _context.BuyingBillItems.RemoveRange(bill.Items);
                _context.BuyingBillPayments.RemoveRange(bill.Payments);
                _context.BuyingBillExpences.RemoveRange(bill.Expences);
                _context.BuyingBills.Remove(bill);

                await _context.SaveChangesAsync();

                return true;
            });
        }

        public Task<ServiceResponse<List<BuyingBillListDto>>> GetAllBuyingBills()
        {
            return ServiceResponse<List<BuyingBillListDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var dateRangeResponse = await _businessYearService.GetSelectedBusinessYearDates();
                if (!dateRangeResponse.Success)
                {
                    err.SetErrors(dateRangeResponse);
                    return default;
                }

                var (startDate, endDate) = dateRangeResponse.Data;

                var bills = await _context.BuyingBills
                    .Include(b => b.Agency)
                    .Include(b => b.Items)
                    .Include(b => b.Payments)
                    .Include(b => b.Expences)
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .OrderByDescending(b => b.Date)
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<BuyingBillListDto>>(bills);
            });
        }

        public Task<ServiceResponse<List<BuyingBillListDto>>> GetAllBuyingBillsByAgencyId(int agencyId)
        {
            return ServiceResponse<List<BuyingBillListDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var dateRangeResponse = await _businessYearService.GetSelectedBusinessYearDates();
                if (!dateRangeResponse.Success)
                {
                    err.SetErrors(dateRangeResponse);
                    return default;
                }

                var (startDate, endDate) = dateRangeResponse.Data;

                var bills = await _context.BuyingBills
                    .Include(b => b.Agency)
                    .Include(b => b.Items)
                    .Include(b => b.Payments)
                    .Include(b => b.Expences)
                    .Where(b => b.AccountId == accountId && b.AgencyId == agencyId && b.Date >= startDate && b.Date <= endDate)
                    .OrderByDescending(b => b.Date)
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<BuyingBillListDto>>(bills);
            });
        }

        public Task<ServiceResponse<List<BuyingBillItemSuggestionDto>>> GetItemSuggestions(int? agencyId)
        {
            return ServiceResponse<List<BuyingBillItemSuggestionDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var query = from bbi in _context.BuyingBillItems
                            join bb in _context.BuyingBills on bbi.BillId equals bb.Id
                            where bb.AccountId == accountId && !string.IsNullOrWhiteSpace(bbi.ItemName)
                            select new { bbi.ItemName, bbi.Price, bb.Date, bbi.Id, bb.AgencyId };

                if (agencyId.HasValue)
                {
                    query = query.Where(x => x.AgencyId == agencyId.Value);
                }

                var items = await query.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id).ToListAsync();

                // Group in-memory to get count and latest price
                var suggestions = items
                    .GroupBy(x => x.ItemName)
                    .Select(g => new BuyingBillItemSuggestionDto
                    {
                        ItemName = g.Key,
                        Count = g.Count(),
                        Price = g.First().Price
                    })
                    .OrderByDescending(s => s.Count)
                    .ToList();

                return suggestions;
            });
        }

        public Task<ServiceResponse<List<string>>> GetExpenceTypeSuggestions()
        {
            return ServiceResponse<List<string>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var suggestions = await (from bbe in _context.BuyingBillExpences
                                         join bb in _context.BuyingBills on bbe.BillId equals bb.Id
                                         where bb.AccountId == accountId && !string.IsNullOrWhiteSpace(bbe.ExpenceType)
                                         select bbe.ExpenceType!)
                                        .GroupBy(x => x.Trim())
                                        .OrderByDescending(g => g.Count())
                                        .Select(g => g.Key)
                                        .ToListAsync();

                return suggestions;
            });
        }
    }
}
