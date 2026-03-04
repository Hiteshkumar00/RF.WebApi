using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.AgencyPerson;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class AgencyPersonService : IAgencyPersonService
    {
        private readonly RFDBContext _context;
        private readonly IMapper _mapper;

        public AgencyPersonService(RFDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<int>> CreateAgencyPerson(CreateAgencyPersonDto dto)
        {
            return await ServiceResponse<int>.Execute(async err =>
            {
                var agencyPerson = _mapper.Map<AgencyPerson>(dto);

                _context.AgencyPersons.Add(agencyPerson);
                await _context.SaveChangesAsync();

                return agencyPerson.Id ?? default;
            });
        }

        public Task<ServiceResponse<AgencyPersonDto>> GetAgencyPersonById(int id)
        {
            return ServiceResponse<AgencyPersonDto>.Execute(async err =>
            {
                var agencyPerson = await _context.AgencyPersons
                    .FirstOrDefaultAsync(ap => ap.Id == id); 

                if (agencyPerson == null)
                {
                    err.AddError(AgencyPersonMessages.NotFound);
                    return default;
                }

                return _mapper.Map<AgencyPersonDto>(agencyPerson);
            });
        }

        public Task<ServiceResponse<bool>> UpdateAgencyPerson(UpdateAgencyPersonDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var agencyPerson = await _context.AgencyPersons
                    .FirstOrDefaultAsync(ap => ap.Id == dto.Id);

                if (agencyPerson == null)
                {
                    err.AddError(AgencyPersonMessages.NotFound);
                    return false;
                }

                _mapper.Map(dto, agencyPerson);
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public Task<ServiceResponse<bool>> DeleteAgencyPerson(int id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var agencyPerson = await _context.AgencyPersons
                    .FirstOrDefaultAsync(ap => ap.Id == id);

                if (agencyPerson == null)
                {
                    err.AddError(AgencyPersonMessages.NotFound);
                    return false;
                }

                _context.AgencyPersons.Remove(agencyPerson);
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public Task<ServiceResponse<List<AgencyPersonDto>>> GetAllAgencyPersons()
        {
            return ServiceResponse<List<AgencyPersonDto>>.Execute(async err =>
            {
                // To list securely we should probably join via Agency -> AccountId
                var query = from ap in _context.AgencyPersons
                            join a in _context.Agencies on ap.AgencyId equals a.Id
                            where a.AccountId == Token.AccountId
                            select ap;

                var agencyPersons = await query
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<AgencyPersonDto>>(agencyPersons);
            });
        }
    }
}
