using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.AccountPerson;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class AccountPersonService : IAccountPersonService
    {
        private readonly RFDBContext _context;
        private readonly IMapper _mapper;

        public AccountPersonService(RFDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<int>> CreateAccountPerson(CreateAccountPersonDto dto)
        {
            return await ServiceResponse<int>.Execute(async err =>
            {
                var accountPerson = _mapper.Map<AccountPerson>(dto);
                accountPerson.AccountId = Token.AccountId;

                _context.AccountPersons.Add(accountPerson);
                await _context.SaveChangesAsync();

                return accountPerson.Id ?? default;
            });
        }

        public Task<ServiceResponse<AccountPersonDto>> GetAccountPersonById(int id)
        {
            return ServiceResponse<AccountPersonDto>.Execute(async err =>
            {
                var accountPerson = await _context.AccountPersons
                    .FirstOrDefaultAsync(ap => ap.Id == id && ap.AccountId == Token.AccountId);

                if (accountPerson == null)
                {
                    err.AddError(AccountPersonMessages.NotFound);
                    return default;
                }

                return _mapper.Map<AccountPersonDto>(accountPerson);
            });
        }

        public Task<ServiceResponse<bool>> UpdateAccountPerson(UpdateAccountPersonDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var accountPerson = await _context.AccountPersons
                    .FirstOrDefaultAsync(ap => ap.Id == dto.Id && ap.AccountId == Token.AccountId);

                if (accountPerson == null)
                {
                    err.AddError(AccountPersonMessages.NotFound);
                    return false;
                }

                _mapper.Map(dto, accountPerson);
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public Task<ServiceResponse<bool>> DeleteAccountPerson(int id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var accountPerson = await _context.AccountPersons
                    .FirstOrDefaultAsync(ap => ap.Id == id && ap.AccountId == Token.AccountId);

                if (accountPerson == null)
                {
                    err.AddError(AccountPersonMessages.NotFound);
                    return false;
                }

                var paymentAccount = await _context.PaymentAccounts
                    .FirstOrDefaultAsync(pa => pa.AccountPersonId == id);

                if (paymentAccount != null)
                {
                    err.AddError(string.Format(AccountPersonMessages.InUseInPaymentAccount, paymentAccount.MethodName));
                    return false;
                }

                _context.AccountPersons.Remove(accountPerson);
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public Task<ServiceResponse<List<AccountPersonDto>>> GetAllAccountPersons()
        {
            return ServiceResponse<List<AccountPersonDto>>.Execute(async err =>
            {
                var accountPersons = await _context.AccountPersons
                    .Where(ap => ap.AccountId == Token.AccountId)
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<AccountPersonDto>>(accountPersons);
            });
        }
    }
}
