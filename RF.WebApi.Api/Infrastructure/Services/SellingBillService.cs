using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.SellingBill;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;


namespace RF.WebApi.Api.Infrastructure.Services
{
    public class SellingBillService : ISellingBillService
    {
        private readonly RFDBContext _context;
        private readonly IMapper _mapper;
        private readonly IBusinessYearService _businessYearService;

        public SellingBillService(RFDBContext context, IMapper mapper, IBusinessYearService businessYearService)
        {
            _context = context;
            _mapper = mapper;
            _businessYearService = businessYearService;
        }

        public async Task<ServiceResponse<int>> CreateSellingBill(CreateSellingBillDto dto)
        {
            return await ServiceResponse<int>.Execute(async err =>
            {
                var bill = _mapper.Map<SellingBill>(dto);
                bill.AccountId = Token.AccountId;
                bill.BillNo = "TEMP_BILL_NO"; // Temporary value to bypass NOT NULL constraint during insert

                _context.SellingBills.Add(bill);

                // Propagate Bill navigation to nested warranties and payments for EF to handle BillId
                foreach (var item in bill.Items)
                {
                    if (item.Warrenty != null)
                    {
                        item.Warrenty.Bill = bill;
                    }
                }

                foreach (var payment in bill.Payments)
                {
                    payment.Bill = bill;
                }

                await _context.SaveChangesAsync();

                // Autogenerate BillNo with ID
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
                    bill.BillNo = $"{initials}{year}SB{bill.Id}";
                    await _context.SaveChangesAsync();
                }

                return bill.Id ?? default;
            });
        }

        public Task<ServiceResponse<SellingBillDto>> GetSellingBillById(int id)
        {
            return ServiceResponse<SellingBillDto>.Execute(async err =>
            {
                var bill = await _context.SellingBills
                    .Include(b => b.Payments)
                    .Include(b => b.Items)
                        .ThenInclude(i => i.Warrenty) // Must explicitly include the 1-to-1 child
                    .FirstOrDefaultAsync(b => b.Id == id && b.AccountId == Token.AccountId);

                if (bill == null)
                {
                    err.AddError(SellingBillMessages.NotFound);
                    return default;
                }

                return _mapper.Map<SellingBillDto>(bill);
            });
        }

        public Task<ServiceResponse<bool>> UpdateSellingBill(UpdateSellingBillDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var bill = await _context.SellingBills
                    .Include(b => b.Payments)
                    .Include(b => b.Items)
                        .ThenInclude(i => i.Warrenty) // Must explicitly include to track updates/deletes
                    .FirstOrDefaultAsync(b => b.Id == dto.Id && b.AccountId == Token.AccountId);

                if (bill == null)
                {
                    err.AddError(SellingBillMessages.NotFound);
                    return false;
                }

                // Sync scalar properties (Items, Payments are ignored in Profile)
                _mapper.Map(dto, bill);
                
                // Generic helper handles Add, Update, and Remove for collections safely
                _context.SyncCollection(bill.Items, dto.Items, (e, d) => d.Id > 0 && e.Id == d.Id, _mapper);
                _context.SyncCollection(bill.Payments, dto.Payments, (e, d) => d.Id > 0 && e.Id == d.Id, _mapper);

                // Propagate Bill navigation to newly added or updated warranties and payments
                foreach (var item in bill.Items)
                {
                    if (item.Warrenty != null)
                    {
                        item.Warrenty.Bill = bill;
                    }
                }

                foreach (var payment in bill.Payments)
                {
                    payment.Bill = bill;
                }

                await _context.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<bool>> DeleteSellingBill(int id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var bill = await _context.SellingBills
                     .Include(b => b.Payments)
                     .Include(b => b.Items)
                        .ThenInclude(i => i.Warrenty) 
                     .FirstOrDefaultAsync(b => b.Id == id && b.AccountId == Token.AccountId);

                if (bill == null)
                {
                    err.AddError(SellingBillMessages.NotFound);
                    return false;
                }

                // Delete Items & their nested Warranties first
                foreach (var item in bill.Items)
                {
                    if (item.Warrenty != null)
                    {
                        _context.SellingItemWarrenties.Remove(item.Warrenty);
                    }
                }

                _context.SellingBillItems.RemoveRange(bill.Items);
                _context.SellingBillPayments.RemoveRange(bill.Payments);
                _context.SellingBills.Remove(bill);

                await _context.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<List<SellingBillListDto>>> GetAllSellingBills()
        {
            return ServiceResponse<List<SellingBillListDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var dateRangeResponse = await _businessYearService.GetSelectedBusinessYearDates();
                if (!dateRangeResponse.Success)
                {
                    err.SetErrors(dateRangeResponse);
                    return default;
                }

                var (startDate, endDate) = dateRangeResponse.Data;

                var bills = await _context.SellingBills
                    .Include(b => b.Payments)
                    .Include(b => b.Items) // Warranties are not needed for List mathematical rollups
                    .Where(b => b.AccountId == accountId && b.Date >= startDate && b.Date <= endDate)
                    .OrderByDescending(b => b.Date)
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<SellingBillListDto>>(bills);
            });
        }

        public Task<ServiceResponse<List<SellingBillItemSuggestionDto>>> GetItemSuggestions()
        {
            return ServiceResponse<List<SellingBillItemSuggestionDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                // Fetch items with their names and prices, joined with bills to check AccountId and get latest date
                var items = await (from sbi in _context.SellingBillItems
                                   join sb in _context.SellingBills on sbi.BillId equals sb.Id
                                   where sb.AccountId == accountId && !string.IsNullOrWhiteSpace(sbi.ItemName)
                                   orderby sb.Date descending, sbi.Id descending
                                   select new { sbi.ItemName, sbi.Price })
                                  .ToListAsync();

                // Group in-memory to get count and latest price
                var suggestions = items
                    .GroupBy(x => x.ItemName)
                    .Select(g => new SellingBillItemSuggestionDto
                    {
                        ItemName = g.Key,
                        Count = g.Count(),
                        Price = g.First().Price
                    })
                    .OrderByDescending(s => s.Count) // Show most popular first
                    .ToList();

                return suggestions;
            });
        }
    }
}
