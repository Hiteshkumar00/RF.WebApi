using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.Helpers;
using RF.WebApi.Api.Application.DTOs.Account;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly RFDBContext _context;
        private readonly IMapper _mapper;
        private readonly ISystemConfigurationService _configService;

        public AccountService(RFDBContext context, IMapper mapper, ISystemConfigurationService configService)
        {
            _context = context;
            _mapper = mapper;
            _configService = configService;
        }

        public async Task<ServiceResponse<int>> CreateAccount(CreateAccountDto createAccountDto)
        {
            return await ServiceResponse<int>.Execute(async err =>
            {
                if (!DateFormatHelper.IsValidFormat(createAccountDto.DateFormat))
                {
                    err.AddError("Invalid Date Format. Please use valid .NET date format strings (e.g., dd-MMMM-yyyy).");
                    return default;
                }
                if (!DateFormatHelper.IsValidFormat(createAccountDto.ShortDateFormat))
                {
                    err.AddError("Invalid Short Date Format. Please use valid .NET date format strings (e.g., dd-MMM-yyyy).");
                    return default;
                }

                var newAccount = _mapper.Map<Account>(createAccountDto);
                _context.Accounts.Add(newAccount);
                await _context.SaveChangesAsync();
                return newAccount.Id ?? default;
            });
        }

        public Task<ServiceResponse<AccountDto>> GetAccountById(int id)
        {
            return ServiceResponse<AccountDto>.Execute(async err =>
            {
                if (!Token.IsSuperAdmin && id != Token.AccountId)
                {
                    err.AddError(UserMessages.UnauthorizedAction);
                    return default;
                }

                var account = await _context.Accounts.FindAsync(id);
                if (account == null)
                {
                    err.AddError(AccountMessages.NotFound);
                    return default;
                }
                return _mapper.Map<AccountDto>(account);
            });
        }

        public Task<ServiceResponse<bool>> UpdateAccount(UpdateAccountDto updateAccountDto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                if (!Token.IsSuperAdmin && updateAccountDto.Id != Token.AccountId)
                {
                    err.AddError(UserMessages.UnauthorizedAction);
                    return false;
                }

                var account = await _context.Accounts.FindAsync(updateAccountDto.Id);
                if (account == null)
                {
                    err.AddError(AccountMessages.NotFound);
                    return false;
                }
                
                if (!DateFormatHelper.IsValidFormat(updateAccountDto.DateFormat))
                {
                    err.AddError("Invalid Date Format. Please use valid .NET date format strings (e.g., dd-MMMM-yyyy).");
                    return false;
                }
                if (!DateFormatHelper.IsValidFormat(updateAccountDto.ShortDateFormat))
                {
                    err.AddError("Invalid Short Date Format. Please use valid .NET date format strings (e.g., dd-MMM-yyyy).");
                    return false;
                }

                _mapper.Map(updateAccountDto, account);
                await _context.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<bool>> DeleteAccount(int id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var account = await _context.Accounts.FindAsync(id);
                if (account == null)
                {
                    err.AddError(AccountMessages.NotFound);
                    return false;
                }

                var canDelete = await _configService.GetConfigurationValueAsBool("EnableDeleteAccount");
                if (!canDelete)
                {
                    err.AddError(AccountMessages.DeletionDisabled);
                    return false;
                }

                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<List<AccountDto>>> GetAllAccounts()
        {
            return ServiceResponse<List<AccountDto>>.Execute(async err =>
            {
                var accounts = await _context.Accounts.AsNoTracking().ToListAsync();
                return _mapper.Map<List<AccountDto>>(accounts);
            });
        }
    }
}
