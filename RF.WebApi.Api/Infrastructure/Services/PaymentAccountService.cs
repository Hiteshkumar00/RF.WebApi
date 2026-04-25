using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.PaymentAccount;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class PaymentAccountService : IPaymentAccountService
    {
        private readonly RFDBContext _context;
        private readonly IMapper _mapper;

        public PaymentAccountService(RFDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<int>> CreatePaymentAccount(CreatePaymentAccountDto dto)
        {
            return await ServiceResponse<int>.Execute(async err =>
            {
                var paymentAccount = _mapper.Map<PaymentAccount>(dto);
                paymentAccount.AccountId = Token.AccountId;

                _context.PaymentAccounts.Add(paymentAccount);
                await _context.SaveChangesAsync();

                return paymentAccount.Id ?? default;
            });
        }

        public Task<ServiceResponse<PaymentAccountDto>> GetPaymentAccountById(int id)
        {
            return ServiceResponse<PaymentAccountDto>.Execute(async err =>
            {
                var paymentAccount = await _context.PaymentAccounts
                    .FirstOrDefaultAsync(p => p.Id == id && p.AccountId == Token.AccountId);

                if (paymentAccount == null)
                {
                    err.AddError(PaymentAccountMessages.NotFound);
                    return default;
                }

                return _mapper.Map<PaymentAccountDto>(paymentAccount);
            });
        }

        public Task<ServiceResponse<bool>> UpdatePaymentAccount(UpdatePaymentAccountDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var paymentAccount = await _context.PaymentAccounts
                    .FirstOrDefaultAsync(p => p.Id == dto.Id && p.AccountId == Token.AccountId);

                if (paymentAccount == null)
                {
                    err.AddError(PaymentAccountMessages.NotFound);
                    return false;
                }

                _mapper.Map(dto, paymentAccount);
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public Task<ServiceResponse<bool>> DeletePaymentAccount(int id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var paymentAccount = await _context.PaymentAccounts
                    .FirstOrDefaultAsync(p => p.Id == id && p.AccountId == Token.AccountId);

                if (paymentAccount == null)
                {
                    err.AddError(PaymentAccountMessages.NotFound);
                    return false;
                }

                // 1. Check Selling Bill Payments
                if (await _context.SellingBillPayments.AnyAsync(p => p.PaymentAccountId == id))
                {
                    err.AddError(PaymentAccountMessages.InUseInSellingBill);
                    return false;
                }

                // 2. Check Buying Bill Payments or Expenses
                if (await _context.BuyingBillPayments.AnyAsync(p => p.PaymentAccountId == id) ||
                    await _context.BuyingBillExpences.AnyAsync(e => e.PaymentAccountId == id))
                {
                    err.AddError(PaymentAccountMessages.InUseInBuyingBill);
                    return false;
                }

                // 3. Check Business Expenses
                if (await _context.BusinessExpencePayments.AnyAsync(p => p.PaymentAccountId == id))
                {
                    err.AddError(PaymentAccountMessages.InUseInExpense);
                    return false;
                }

                // 4. Check Contributions
                if (await _context.AddContributionPayments.AnyAsync(p => p.PaymentAccountId == id) ||
                    await _context.RemoveContributionPayments.AnyAsync(p => p.PaymentAccountId == id))
                {
                    err.AddError(PaymentAccountMessages.InUseInContribution);
                    return false;
                }

                _context.PaymentAccounts.Remove(paymentAccount);
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public Task<ServiceResponse<List<PaymentAccountDto>>> GetAllPaymentAccounts()
        {
            return ServiceResponse<List<PaymentAccountDto>>.Execute(async err =>
            {
                var paymentAccounts = await _context.PaymentAccounts
                    .Where(p => p.AccountId == Token.AccountId)
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<PaymentAccountDto>>(paymentAccounts);
            });
        }
    }
}
