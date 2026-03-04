using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.Agency;
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

        public AgencyService(RFDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
    }
}
