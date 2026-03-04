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

                _context.BuyingBills.Add(bill);
                await _context.SaveChangesAsync();

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

                // AutoMapper seamlessly handles syncing all scalar properties
                // And adding/updating/deleting the nested Collections (Items, Payments, Expences)
                _mapper.Map(dto, bill);

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
    }
}
