using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.Agency;
using RF.WebApi.Api.Application.DTOs.AgencyPerson;
using RF.WebApi.Api.Application.DTOs.BuyingBill;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class AgencyService : IAgencyService
    {
        private readonly RFDBContext _context;
        private readonly IMapper _mapper;
        private readonly IBusinessYearService _businessYearService;

        public AgencyService(RFDBContext context, IMapper mapper, IBusinessYearService businessYearService)
        {
            _context = context;
            _mapper = mapper;
            _businessYearService = businessYearService;
        }

        public async Task<ServiceResponse<int>> CreateAgency(CreateAgencyDto dto)
        {
            return await ServiceResponse<int>.Execute(async err =>
            {
                var agency = _mapper.Map<Agency>(dto);
                agency.AccountId = Token.AccountId;

                _context.Agencies.Add(agency);
                await _context.SaveChangesAsync();

                return agency.Id ?? default;
            });
        }

        public Task<ServiceResponse<AgencyDto>> GetAgencyById(int id)
        {
            return ServiceResponse<AgencyDto>.Execute(async err =>
            {
                var agency = await _context.Agencies
                    .FirstOrDefaultAsync(a => a.Id == id && a.AccountId == Token.AccountId);

                if (agency == null)
                {
                    err.AddError(AgencyMessages.NotFound);
                    return default;
                }

                return _mapper.Map<AgencyDto>(agency);
            });
        }

        public Task<ServiceResponse<bool>> UpdateAgency(UpdateAgencyDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var agency = await _context.Agencies
                    .FirstOrDefaultAsync(a => a.Id == dto.Id && a.AccountId == Token.AccountId);

                if (agency == null)
                {
                    err.AddError(AgencyMessages.NotFound);
                    return false;
                }

                _mapper.Map(dto, agency);
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public Task<ServiceResponse<bool>> DeleteAgency(int id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var agency = await _context.Agencies
                    .FirstOrDefaultAsync(a => a.Id == id && a.AccountId == Token.AccountId);

                if (agency == null)
                {
                    err.AddError(AgencyMessages.NotFound);
                    return false;
                }

                var bill = await _context.BuyingBills
                    .FirstOrDefaultAsync(b => b.AgencyId == id);

                if (bill != null)
                {
                    err.AddError(string.Format(AgencyMessages.InUseInBuyingBill, bill.BillNo));
                    return false;
                }

                _context.Agencies.Remove(agency);
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public Task<ServiceResponse<List<AgencyDto>>> GetAllAgencies()
        {
            return ServiceResponse<List<AgencyDto>>.Execute(async err =>
            {
                var agencies = await _context.Agencies
                    .Where(a => a.AccountId == Token.AccountId)
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<AgencyDto>>(agencies);
            });
        }

        
        public Task<ServiceResponse<List<AgencyAdvancedListDto>>> GetAllAgencysAdvancedAsync()
        {
            return ServiceResponse<List<AgencyAdvancedListDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var agencies = await _context.Agencies
                    .Where(a => a.AccountId == accountId)
                    .AsNoTracking()
                    .ToListAsync();

                var results = new List<AgencyAdvancedListDto>();

                foreach (var agency in agencies)
                {
                    var dto = _mapper.Map<AgencyAdvancedListDto>(agency);

                    // 1. Total Bills Amount (Items + Expences - Discounts for this specific agency)
                    var totalItems = await _context.BuyingBills
                        .Where(b => b.AccountId == accountId && b.AgencyId == agency.Id)
                        .SelectMany(b => b.Items)
                        .SumAsync(i => (i.Quantity ?? 0) * (i.Price ?? 0));

                    var totalExpences = await _context.BuyingBills
                        .Where(b => b.AccountId == accountId && b.AgencyId == agency.Id)
                        .SelectMany(b => b.Expences)
                        .SumAsync(e => e.Amount ?? 0);

                    var totalDiscounts = await _context.BuyingBills
                        .Where(b => b.AccountId == accountId && b.AgencyId == agency.Id)
                        .SumAsync(b => b.Discount ?? 0);

                    dto.TotalBillsAmount = totalItems + totalExpences - totalDiscounts;

                    // 2. Total Paid Amount
                    dto.TotalPaidAmount = await _context.BuyingBillPayments
                        .Where(p => p.BillId != null && _context.BuyingBills.Any(b => b.Id == p.BillId && b.AccountId == accountId && b.AgencyId == agency.Id))
                        .SumAsync(p => p.Amount ?? 0);

                    // 3. Total Pending Amount
                    dto.TotalPendingAmount = dto.TotalBillsAmount - dto.TotalPaidAmount;

                    results.Add(dto);
                }

                return results;
            });
        }

        public Task<ServiceResponse<ViewAgencyAllDetailDto>> GetAgencyAllDetailAsync(int agencyId)
        {
            return ServiceResponse<ViewAgencyAllDetailDto>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var agency = await _context.Agencies
                    .FirstOrDefaultAsync(a => a.Id == agencyId && a.AccountId == accountId);

                if (agency == null)
                {
                    err.AddError(AgencyMessages.NotFound);
                    return default;
                }

                var dto = _mapper.Map<ViewAgencyAllDetailDto>(agency);

                // --- 1. Top Level Totals ---
                var totalItems = await _context.BuyingBills
                    .Where(b => b.AccountId == accountId && b.AgencyId == agencyId)
                    .SelectMany(b => b.Items)
                    .SumAsync(i => (i.Quantity ?? 0) * (i.Price ?? 0));

                var totalExpences = await _context.BuyingBills
                    .Where(b => b.AccountId == accountId && b.AgencyId == agencyId)
                    .SelectMany(b => b.Expences)
                    .SumAsync(e => e.Amount ?? 0);

                var totalDiscounts = await _context.BuyingBills
                    .Where(b => b.AccountId == accountId && b.AgencyId == agencyId)
                    .SumAsync(b => b.Discount ?? 0);

                dto.TotalBillsAmount = totalItems + totalExpences - totalDiscounts;

                dto.TotalPaidAmount = await _context.BuyingBillPayments
                    .Where(p => p.BillId != null && _context.BuyingBills.Any(b => b.Id == p.BillId && b.AccountId == accountId && b.AgencyId == agencyId))
                    .SumAsync(p => p.Amount ?? 0);

                dto.TotalPendingAmount = dto.TotalBillsAmount - dto.TotalPaidAmount;


                // --- 2. List of Agency Persons ---
                var persons = await _context.AgencyPersons
                    .Where(p => p.AgencyId == agencyId)
                    .ToListAsync();
                dto.AgencyPersons = _mapper.Map<List<AgencyPersonDto>>(persons);


                // --- 3. List of Bills Grouped By Year ---
                var dateRangeResponse = await _businessYearService.GetAllBusinessYearDateRanges();
                if (!dateRangeResponse.Success)
                {
                    err.SetErrors(dateRangeResponse);
                    return default;
                }

                // First, fetch the raw flattened bills to avoid N+1 querying in loops
                var billsResult = await _context.BuyingBills
                    .Include(b => b.Agency)
                    .Include(b => b.Items)
                    .Include(b => b.Payments)
                    .Include(b => b.Expences)
                    .Where(b => b.AccountId == accountId && b.AgencyId == agencyId)
                    .OrderByDescending(b => b.Date)
                    .ToListAsync();

                var allBills = _mapper.Map<List<BuyingBillListDto>>(billsResult);

                // Bucket them into logic intervals
                var usedBillIds = new HashSet<int>();

                if (dateRangeResponse.Data != null)
                {
                    foreach (var range in dateRangeResponse.Data)
                    {
                        var matchingBills = allBills.Where(b => b.Date >= range.StartDate && b.Date <= range.EndDate).ToList();
                        
                        if (matchingBills.Any())
                        {
                            dto.BillsByYear.Add(new AgencyBillsByYearDto
                            {
                                BusinessYearId = range.Id,
                                YearName = range.YearName,
                                StartDate = range.StartDate,
                                EndDate = range.EndDate,
                                Bills = matchingBills
                            });

                            foreach (var b in matchingBills) usedBillIds.Add(b.Id);
                        }
                    }
                }

                // Bucket any legacy bills mapping outside named boundaries
                var leftOverBills = allBills.Where(b => !usedBillIds.Contains(b.Id)).ToList();
                if (leftOverBills.Any())
                {
                    dto.BillsByYear.Add(new AgencyBillsByYearDto
                    {
                        BusinessYearId = null,
                        YearName = "Other (Out of bounds)",
                        Bills = leftOverBills
                    });
                }

                return dto;
            });
        }
    }
}
