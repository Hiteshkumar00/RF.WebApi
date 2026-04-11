using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.SellingBill;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;
using RF.WebApi.Infrastructure.Data.DataBase.Extensions;

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

                _context.SellingBills.Add(bill);
                await _context.SaveChangesAsync();

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

                _mapper.Map(dto, bill);

                // Sync collections
                _context.SyncCollection(bill.Items, dto.Items, (e, d) => e.Id == d.Id, _mapper);
                _context.SyncCollection(bill.Payments, dto.Payments, (e, d) => e.Id == d.Id, _mapper);

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
    }
}
