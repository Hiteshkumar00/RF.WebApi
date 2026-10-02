using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.AgencyPerson;
using RF.WebApi.Api.Application.DTOs.Common;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class AgencyPersonService : BaseService, IAgencyPersonService
    {
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;

        public AgencyPersonService(RFDBContext context, IMapper mapper, IExcelService excelService) : base(context)
        {
            _mapper = mapper;
            _excelService = excelService;
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
                var query = from ap in _context.AgencyPersons
                            join a in _context.Agencies on ap.AgencyId equals a.Id
                            where a.AccountId == Token.AccountId
                            select new AgencyPersonDto
                            {
                                Id = ap.Id ?? 0,
                                AgencyId = ap.AgencyId ?? 0,
                                AgencyName = a.AgencyName,
                                Name = ap.Name,
                                PhoneNo = ap.PhoneNo,
                                Email = ap.Email,
                                PersonOccupation = ap.PersonOccupation,
                                Address = ap.Address
                            };

                var dtos = await query.ToListAsync();
                return dtos;
            });
        }

        public Task<ServiceResponse<byte[]>> ExportAgencyPersons()
        {
            return ServiceResponse<byte[]>.Execute(async err =>
            {
                var query = from ap in _context.AgencyPersons
                            join a in _context.Agencies on ap.AgencyId equals a.Id
                            where a.AccountId == Token.AccountId
                            select new AgencyPersonDto
                            {
                                Id = ap.Id ?? 0,
                                AgencyId = ap.AgencyId ?? 0,
                                AgencyName = a.AgencyName,
                                Name = ap.Name,
                                PhoneNo = ap.PhoneNo,
                                Email = ap.Email,
                                PersonOccupation = ap.PersonOccupation,
                                Address = ap.Address
                            };

                var dtos = await query.ToListAsync();
                return _excelService.Export(dtos, "Agency Persons");
            });
        }

        public Task<ServiceResponse<ImportResultDto>> ImportAgencyPersons(List<ImportAgencyPersonDto> dtos)
        {
            return ServiceResponse<ImportResultDto>.Execute(async err =>
            {
                var result = new ImportResultDto();
                var accountId = Token.AccountId;
                var agencies = await _context.Agencies
                    .Where(a => a.AccountId == accountId)
                    .ToDictionaryAsync(a => a.AgencyName ?? "", a => a.Id);

                var toAdd = new List<AgencyPerson>();

                foreach(var dto in dtos)
                {
                    dto.AgencyName = dto.AgencyName?.Trim();
                    dto.Name = dto.Name?.Trim();

                    if (!agencies.TryGetValue(dto.AgencyName ?? "", out var agencyId))
                    {
                        result.Errors.Add(new ImportRowMessageDto { RowNo = dto.RowNo, Message = $"Agency '{dto.AgencyName}' not found for person '{dto.Name}'." });
                        continue;
                    }



                    var person = new AgencyPerson 
                    {
                        AgencyId = agencyId,
                        Name = dto.Name,
                        PhoneNo = dto.PhoneNo,
                        Email = dto.Email,
                        PersonOccupation = dto.PersonOccupation,
                        Address = dto.Address
                    };
                    toAdd.Add(person);
                    result.SuccessCount++;
                }
                
                if (result.Errors.Count > 0)
                {
                    result.SuccessCount = 0;
                    return result;
                }

                await RunInTransactionAsync(async () =>
                {
                    _context.AgencyPersons.AddRange(toAdd);
                });

                return result;
            });
        }
    }
}
