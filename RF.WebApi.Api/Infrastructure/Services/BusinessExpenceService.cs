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
                    .FirstOrDefaultAsync(e => e.Id == id && e.AccountId == Token.AccountId);

                if (expense == null)
                {
                    err.AddError(BusinessExpenceMessages.NotFound);
                    return default;
                }

                return _mapper.Map<BusinessExpenceDto>(expense);
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

                _mapper.Map(dto, expense);

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

        public Task<ServiceResponse<List<BusinessExpenceDto>>> GetAllBusinessExpences()
        {
            return ServiceResponse<List<BusinessExpenceDto>>.Execute(async err =>
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

                // Fetch Business Expences with included payments within this range
                var expenses = await _context.BusinessExpences
                    .Include(e => e.Payments)
                    .Where(e => e.AccountId == accountId && e.Date >= startDate && e.Date <= endDate)
                    .OrderByDescending(e => e.Date)
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<BusinessExpenceDto>>(expenses);
            });
        }
    }
}
