using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.BusinessYear;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;
using System.Security.Claims;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class BusinessYearService : IBusinessYearService
    {
        private readonly RFDBContext _context;
        private readonly IMapper _mapper;

        public BusinessYearService(RFDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<int>> CreateBusinessYear(CreateBusinessYearDto dto)
        {
            return await ServiceResponse<int>.Execute(async err =>
            {
                var businessYear = _mapper.Map<BusinessYear>(dto);
                businessYear.AccountId = Token.AccountId;

                _context.BusinessYears.Add(businessYear);
                await _context.SaveChangesAsync();

                return businessYear.Id ?? default;
            });
        }

        public Task<ServiceResponse<bool>> UpdateBusinessYear(UpdateBusinessYearDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var businessYear = await _context.BusinessYears
                    .FirstOrDefaultAsync(by => by.Id == dto.Id && by.AccountId == Token.AccountId);

                if (businessYear == null)
                {
                    err.AddError(BusinessYearMessages.NotFound);
                    return false;
                }

                _mapper.Map(dto, businessYear);
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public Task<ServiceResponse<bool>> DeleteBusinessYear(int id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var businessYear = await _context.BusinessYears
                    .FirstOrDefaultAsync(by => by.Id == id && by.AccountId == Token.AccountId);

                if (businessYear == null)
                {
                    err.AddError(BusinessYearMessages.NotFound);
                    return false;
                }

                _context.BusinessYears.Remove(businessYear);
                
                // Cleanup associated user mappings
                var mappings = await _context.UserSelectedYearMappings
                    .Where(m => m.BusinessYearId == id)
                    .ToListAsync();
                if (mappings.Any())
                {
                    _context.UserSelectedYearMappings.RemoveRange(mappings);
                }

                await _context.SaveChangesAsync();
                return true;
            });
        }

        public async Task<ServiceResponse<List<BusinessYearListDto>>> GetAllBusinessYears()
        {
            return await ServiceResponse<List<BusinessYearListDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;
                var userId = Token.UserId;

                // 1. Fetch the computed DateRanges for this account
                var dateRangesResponse = await GetAllBusinessYearDateRanges();
                if (!dateRangesResponse.Success)
                {
                    err.SetErrors(dateRangesResponse);
                    return default;
                }

                var dateRanges = dateRangesResponse.Data;
                var dtoList = new List<BusinessYearListDto>();

                if (dateRanges == null || !dateRanges.Any())
                {
                    return dtoList;
                }

                // Convert DateRanges to BusinessYearListDto
                foreach (var range in dateRanges)
                {
                    dtoList.Add(new BusinessYearListDto
                    {
                        Id = range.Id,
                        AccountId = accountId,
                        YearName = range.YearName,
                        StartDate = range.StartDate,
                        EndDate = range.EndDate,
                        IsSelected = false 
                    });
                }

                // 2. Fetch the User's selected year mapping
                var userMapping = await _context.UserSelectedYearMappings
                    .FirstOrDefaultAsync(m => m.UserId == userId && m.AccountId == accountId);

                if (userMapping != null)
                {
                    // 3. If a mapping exists, set IsSelected = true for that specific year
                    var selectedDto = dtoList.FirstOrDefault(dto => dto.Id == userMapping.BusinessYearId);
                    if (selectedDto != null)
                    {
                        selectedDto.IsSelected = true;
                    }
                    else
                    {
                        // Fallback: If mapping exists but the year was somehow deleted, default to the last one
                        dtoList.Last().IsSelected = true;
                    }
                }
                else
                {
                    // 4. Default: No mapping exists, select the latest year (which is the last one in an ASC sorted list)
                    dtoList.Last().IsSelected = true;
                }

                return dtoList;
            });
        }

        public Task<ServiceResponse<bool>> ChangeSelectedYear(ChangeUserSelectedYearDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var accountId = Token.AccountId;
                var userId = Token.UserId;

                // Validate the requested business year belongs to this account
                var yearExists = await _context.BusinessYears
                    .AnyAsync(by => by.Id == dto.BusinessYearId && by.AccountId == accountId);

                if (!yearExists)
                {
                    err.AddError(BusinessYearMessages.NotFound);
                    return false;
                }

                // Find existing mapping
                var mapping = await _context.UserSelectedYearMappings
                    .FirstOrDefaultAsync(m => m.UserId == userId && m.AccountId == accountId);

                if (mapping != null)
                {
                    // Update existing mapping
                    mapping.BusinessYearId = dto.BusinessYearId;
                }
                else
                {
                    // Create new mapping
                    mapping = new UserSelectedYearMapping
                    {
                        UserId = userId,
                        AccountId = accountId,
                        BusinessYearId = dto.BusinessYearId
                    };
                    _context.UserSelectedYearMappings.Add(mapping);
                }

                await _context.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<(DateOnly StartDate, DateOnly EndDate)>> GetSelectedBusinessYearDates()
        {
            return ServiceResponse<(DateOnly StartDate, DateOnly EndDate)>.Execute(async err =>
            {
                var accountId = Token.AccountId;
                var userId = Token.UserId;

                // 1. Get ordered list of business years for the account
                var businessYears = await _context.BusinessYears
                    .Where(by => by.AccountId == accountId)
                    .OrderBy(by => by.Date)
                    .AsNoTracking()
                    .ToListAsync();

                if (!businessYears.Any())
                {
                    err.AddError("No Business Years configured for this account.");
                    return default;
                }

                // 2. Identify selected year
                var userMapping = await _context.UserSelectedYearMappings
                    .FirstOrDefaultAsync(m => m.UserId == userId && m.AccountId == accountId);

                BusinessYear? selectedYear = null;

                if (userMapping != null)
                {
                    selectedYear = businessYears.FirstOrDefault(by => by.Id == userMapping.BusinessYearId);
                }

                // Fallback to the latest year if mapping missing or invalid
                if (selectedYear == null)
                {
                    selectedYear = businessYears.Last();
                }

                var startDate = selectedYear.Date!.Value;
                DateOnly endDate;

                // 3. Find next sequential year
                var nextYear = businessYears.FirstOrDefault(by => by.Date > startDate);

                if (nextYear != null && nextYear.Date.HasValue)
                {
                    // End date is one day before the next year's start date
                    endDate = nextYear.Date.Value.AddDays(-1);
                }
                else
                {
                    // If no next year, till now
                    endDate = DateOnly.FromDateTime(DateTime.Now);
                }

                return (startDate, endDate);
            });
        }

        public Task<ServiceResponse<List<BusinessYearDateRangeDto>>> GetAllBusinessYearDateRanges()
        {
            return ServiceResponse<List<BusinessYearDateRangeDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var businessYears = await _context.BusinessYears
                    .Where(by => by.AccountId == accountId)
                    .OrderBy(by => by.Date)
                    .AsNoTracking()
                    .ToListAsync();

                var results = new List<BusinessYearDateRangeDto>();

                for (int i = 0; i < businessYears.Count; i++)
                {
                    var currentYear = businessYears[i];

                    if (!currentYear.Date.HasValue) continue;

                    var startDate = currentYear.Date.Value;
                    DateOnly endDate;

                    if (i + 1 < businessYears.Count && businessYears[i + 1].Date.HasValue)
                    {
                        endDate = businessYears[i + 1].Date!.Value.AddDays(-1);
                    }
                    else
                    {
                        endDate = DateOnly.FromDateTime(DateTime.Now);
                    }

                    results.Add(new BusinessYearDateRangeDto
                    {
                        Id = currentYear.Id ?? 0,
                        YearName = currentYear.YearName ?? "Unknown",
                        StartDate = startDate,
                        EndDate = endDate
                    });
                }

                return results;
            });
        }
    }
}
