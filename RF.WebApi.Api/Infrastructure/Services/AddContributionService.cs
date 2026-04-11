using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.Contribution;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;
using RF.WebApi.Infrastructure.Data.DataBase.Extensions;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class AddContributionService : IAddContributionService
    {
        private readonly RFDBContext _context;
        private readonly IMapper _mapper;
        private readonly IBusinessYearService _businessYearService;

        public AddContributionService(RFDBContext context, IMapper mapper, IBusinessYearService businessYearService)
        {
            _context = context;
            _mapper = mapper;
            _businessYearService = businessYearService;
        }

        private async Task<bool> ValidateAccountPersonOwnership(int accountPersonId)
        {
            return await _context.AccountPersons.AnyAsync(ap => ap.Id == accountPersonId && ap.AccountId == Token.AccountId);
        }

        public Task<ServiceResponse<int>> CreateAddContribution(CreateAddContributionDto dto)
        {
            return ServiceResponse<int>.Execute(async err =>
            {
                if (!await ValidateAccountPersonOwnership(dto.AccountPersonId))
                {
                    err.AddError(ContributionMessages.UnauthorizedAccess);
                    return default;
                }

                var contribution = _mapper.Map<AddContribution>(dto);
                _context.AddContributions.Add(contribution);
                await _context.SaveChangesAsync();

                return contribution.Id ?? default;
            });
        }

        public Task<ServiceResponse<AddContributionDto>> GetAddContributionById(int id)
        {
            return ServiceResponse<AddContributionDto>.Execute(async err =>
            {
                var contribution = await _context.AddContributions
                    .Include(c => c.Payments)
                    .FirstOrDefaultAsync(c => c.Id == id && 
                        _context.AccountPersons.Any(ap => ap.Id == c.AccountPersonId && ap.AccountId == Token.AccountId));

                if (contribution == null)
                {
                    err.AddError(ContributionMessages.NotFound);
                    return default;
                }

                return _mapper.Map<AddContributionDto>(contribution);
            });
        }

        public Task<ServiceResponse<bool>> UpdateAddContribution(UpdateAddContributionDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var contribution = await _context.AddContributions
                    .Include(c => c.Payments)
                    .FirstOrDefaultAsync(c => c.Id == dto.Id && 
                        _context.AccountPersons.Any(ap => ap.Id == c.AccountPersonId && ap.AccountId == Token.AccountId));

                if (contribution == null)
                {
                    err.AddError(ContributionMessages.NotFound);
                    return false;
                }

                // If they change the AccountPersonId, we must validate the new one
                if (contribution.AccountPersonId != dto.AccountPersonId)
                {
                    if (!await ValidateAccountPersonOwnership(dto.AccountPersonId))
                    {
                        err.AddError(ContributionMessages.UnauthorizedAccess);
                        return false;
                    }
                }

                // Sync scalar properties (Payments is ignored in Profile)
                _mapper.Map(dto, contribution);
                
                // Generic helper handles Add, Update, and Remove for collection safely
                _context.SyncCollection(contribution.Payments, dto.Payments, (e, d) => d.Id > 0 && e.Id == d.Id, _mapper);
                await _context.SaveChangesAsync();
                
                return true;
            });
        }

        public Task<ServiceResponse<bool>> DeleteAddContribution(int id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var contribution = await _context.AddContributions
                    .Include(c => c.Payments)
                    .FirstOrDefaultAsync(c => c.Id == id && 
                        _context.AccountPersons.Any(ap => ap.Id == c.AccountPersonId && ap.AccountId == Token.AccountId));

                if (contribution == null)
                {
                    err.AddError(ContributionMessages.NotFound);
                    return false;
                }

                _context.AddContributionPayments.RemoveRange(contribution.Payments);
                _context.AddContributions.Remove(contribution);
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public Task<ServiceResponse<List<AddContributionListDto>>> GetAllAddContributions()
        {
            return ServiceResponse<List<AddContributionListDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var dateRangeResponse = await _businessYearService.GetSelectedBusinessYearDates();
                if (!dateRangeResponse.Success)
                {
                    err.SetErrors(dateRangeResponse);
                    return default;
                }

                var (startDate, endDate) = dateRangeResponse.Data;

                var contributions = await _context.AddContributions
                    .Include(c => c.AccountPerson)
                    .Include(c => c.Payments)
                    .Where(c => _context.AccountPersons.Any(ap => ap.Id == c.AccountPersonId && ap.AccountId == accountId) &&
                                c.Date >= startDate && c.Date <= endDate)
                    .OrderByDescending(c => c.Date)
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<AddContributionListDto>>(contributions);
            });
        }

        public Task<ServiceResponse<List<AddContributionListDto>>> GetAllAddContributionsByAccountPersonId(int accountPersonId)
        {
            return ServiceResponse<List<AddContributionListDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var dateRangeResponse = await _businessYearService.GetSelectedBusinessYearDates();
                if (!dateRangeResponse.Success)
                {
                    err.SetErrors(dateRangeResponse);
                    return default;
                }

                var (startDate, endDate) = dateRangeResponse.Data;

                var contributions = await _context.AddContributions
                    .Include(c => c.AccountPerson)
                    .Include(c => c.Payments)
                    .Where(c => c.AccountPersonId == accountPersonId &&
                                _context.AccountPersons.Any(ap => ap.Id == c.AccountPersonId && ap.AccountId == accountId) &&
                                c.Date >= startDate && c.Date <= endDate)
                    .OrderByDescending(c => c.Date)
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<AddContributionListDto>>(contributions);
            });
        }
    }
}
