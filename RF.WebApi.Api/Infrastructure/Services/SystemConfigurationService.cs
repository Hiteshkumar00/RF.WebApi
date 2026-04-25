using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.SystemConfiguration;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Infrastructure.Data.DataBase;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class SystemConfigurationService : ISystemConfigurationService
    {
        private readonly RFDBContext _context;
        private readonly IMapper _mapper;

        public SystemConfigurationService(RFDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<SystemConfigurationDto>>> GetAllConfigurations()
        {
            return await ServiceResponse<List<SystemConfigurationDto>>.Execute(async err =>
            {
                var configs = await _context.SystemConfigurations
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<SystemConfigurationDto>>(configs);
            });
        }

        public async Task<ServiceResponse<bool>> UpdateConfiguration(UpdateSystemConfigurationDto dto)
        {
            return await ServiceResponse<bool>.Execute(async err =>
            {
                // Security Check: Only the root superadmin (UserId -1) can update system configuration
                if (Token.UserId != -1)
                {
                    err.AddError(UserMessages.UnauthorizedAction);
                    return false;
                }

                var config = await _context.SystemConfigurations
                    .FirstOrDefaultAsync(c => c.Id == dto.Id);

                if (config == null)
                {
                    err.AddError("Configuration not found");
                    return false;
                }

                config.PropertyValue = dto.PropertyValue;
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public async Task<bool> GetConfigurationValueAsBool(string propertyName)
        {
            var value = await GetConfigurationValueAsString(propertyName);
            return bool.TryParse(value, out bool result) && result;
        }

        public async Task<string> GetConfigurationValueAsString(string propertyName)
        {
            var config = await _context.SystemConfigurations
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.PropertyName == propertyName);

            return config?.PropertyValue ?? string.Empty;
        }
    }
}
