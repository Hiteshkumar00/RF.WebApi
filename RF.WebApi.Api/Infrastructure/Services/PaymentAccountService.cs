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

                // 2. Check Buying Bill Payments
                if (await _context.BuyingBillPayments.AnyAsync(p => p.PaymentAccountId == id))
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

                // 5. Check Transfers
                if (await _context.PaymentTransfers.AnyAsync(p => p.FromPaymentAccountId == id || p.ToPaymentAccountId == id))
                {
                    err.AddError(PaymentAccountMessages.InUseInTransfer);
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

        public Task<ServiceResponse<List<PaymentHistoryDto>>> GetPaymentHistory(PaymentHistoryFilterDto filter)
        {
            return ServiceResponse<List<PaymentHistoryDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;
                var history = new List<PaymentHistoryDto>();

                // 1. Selling Bill Payments (Received)
                if (string.IsNullOrEmpty(filter.Direction) || filter.Direction == "Received")
                {
                    if (string.IsNullOrEmpty(filter.PaymentType) || filter.PaymentType == "Selling Bill")
                    {
                        var payments = await _context.SellingBillPayments
                            .Include(p => p.Bill)
                            .Include(p => p.PaymentAccount)
                            .Where(p => p.Bill.AccountId == accountId)
                            .Where(p => filter.PaymentAccountId == null || p.PaymentAccountId == filter.PaymentAccountId)
                            .Where(p => filter.FromDate == null || p.Date >= filter.FromDate)
                            .Where(p => filter.ToDate == null || p.Date <= filter.ToDate)
                            .Select(p => new PaymentHistoryDto
                            {
                                Id = p.Id ?? 0,
                                PaymentAccountName = p.PaymentAccount != null ? p.PaymentAccount.MethodName ?? "" : "",
                                Description = p.Bill.CustomerName ?? "",
                                Direction = "Received",
                                Amount = p.Amount ?? 0,
                                Date = p.Date ?? p.Bill.Date ?? default,
                                PaymentType = "Selling Bill",
                                BillNo = p.Bill.BillNo
                            })
                            .ToListAsync();
                        history.AddRange(payments);
                    }
                }

                // 2. Buying Bill Payments (Paid)
                if (string.IsNullOrEmpty(filter.Direction) || filter.Direction == "Paid")
                {
                    if (string.IsNullOrEmpty(filter.PaymentType) || filter.PaymentType == "Buying Bill")
                    {
                        var payments = await _context.BuyingBillPayments
                            .Include(p => p.Bill)
                                .ThenInclude(b => b.Agency)
                            .Include(p => p.PaymentAccount)
                            .Where(p => p.Bill.AccountId == accountId)
                            .Where(p => filter.PaymentAccountId == null || p.PaymentAccountId == filter.PaymentAccountId)
                            .Where(p => filter.FromDate == null || p.Date >= filter.FromDate)
                            .Where(p => filter.ToDate == null || p.Date <= filter.ToDate)
                            .Select(p => new PaymentHistoryDto
                            {
                                Id = p.Id ?? 0,
                                PaymentAccountName = p.PaymentAccount != null ? p.PaymentAccount.MethodName ?? "" : "",
                                Description = p.Bill.Agency != null ? p.Bill.Agency.AgencyName ?? "" : "",
                                Direction = "Paid",
                                Amount = p.Amount ?? 0,
                                Date = p.Date ?? p.Bill.Date ?? default,
                                PaymentType = "Buying Bill",
                                BillNo = p.Bill.BillNo
                            })
                            .ToListAsync();
                        history.AddRange(payments);
                    }
                }

                // 3. Business Expense Payments (Paid)
                if (string.IsNullOrEmpty(filter.Direction) || filter.Direction == "Paid")
                {
                    var query = _context.BusinessExpencePayments
                        .Include(p => p.BusinessExpence)
                            .ThenInclude(e => e.BuyingBill)
                                .ThenInclude(b => b.Agency)
                        .Include(p => p.PaymentAccount)
                        .Where(p => p.BusinessExpence.AccountId == accountId)
                        .Where(p => filter.PaymentAccountId == null || p.PaymentAccountId == filter.PaymentAccountId)
                        .Where(p => filter.FromDate == null || p.Date >= filter.FromDate)
                        .Where(p => filter.ToDate == null || p.Date <= filter.ToDate)
                        .AsQueryable();

                    var rawExpensePayments = await query.ToListAsync();

                    var expenseHistory = rawExpensePayments
                        .Where(p => (string.IsNullOrEmpty(filter.PaymentType)) ||
                                   (filter.PaymentType == "Expense" && p.BusinessExpence.BuyingBillId == null) ||
                                   (filter.PaymentType == "Buying Bill Expense" && p.BusinessExpence.BuyingBillId != null))
                        .Select(p => new PaymentHistoryDto
                        {
                            Id = p.Id ?? 0,
                            PaymentAccountName = p.PaymentAccount != null ? p.PaymentAccount.MethodName ?? "" : "",
                            Description = p.BusinessExpence.BuyingBillId != null
                                ? (p.BusinessExpence.BuyingBill?.Agency?.AgencyName ?? "")
                                : (p.BusinessExpence.ExpenceType ?? ""),
                            Direction = "Paid",
                            Amount = p.Amount ?? 0,
                            Date = p.Date ?? p.BusinessExpence.Date ?? default,
                            PaymentType = p.BusinessExpence.BuyingBillId != null ? "Buying Bill Expense" : "Expense",
                            BillNo = p.BusinessExpence.BuyingBill?.BillNo
                        });
                    history.AddRange(expenseHistory);
                }

                // 4. Add Contribution (Received)
                if (string.IsNullOrEmpty(filter.Direction) || filter.Direction == "Received")
                {
                    if (string.IsNullOrEmpty(filter.PaymentType) || filter.PaymentType == "Add Contribution")
                    {
                        var payments = await _context.AddContributionPayments
                            .Include(p => p.AddContribution)
                                .ThenInclude(c => c.AccountPerson)
                            .Include(p => p.PaymentAccount)
                            .Where(p => p.AddContribution.AccountId == accountId)
                            .Where(p => filter.PaymentAccountId == null || p.PaymentAccountId == filter.PaymentAccountId)
                            .Where(p => filter.FromDate == null || p.Date >= filter.FromDate)
                            .Where(p => filter.ToDate == null || p.Date <= filter.ToDate)
                            .Select(p => new PaymentHistoryDto
                            {
                                Id = p.Id ?? 0,
                                PaymentAccountName = p.PaymentAccount != null ? p.PaymentAccount.MethodName ?? "" : "",
                                AccountPersonName = p.AddContribution.AccountPerson != null ? p.AddContribution.AccountPerson.Name : "",
                                Description = p.AddContribution.Description ?? "",
                                Direction = "Received",
                                Amount = p.Amount ?? 0,
                                Date = p.Date ?? p.AddContribution.Date ?? default,
                                PaymentType = "Add Contribution",
                                BillNo = null
                            })
                            .ToListAsync();
                        history.AddRange(payments);
                    }
                }

                // 5. Remove Contribution (Paid)
                if (string.IsNullOrEmpty(filter.Direction) || filter.Direction == "Paid")
                {
                    if (string.IsNullOrEmpty(filter.PaymentType) || filter.PaymentType == "Remove Contribution")
                    {
                        var payments = await _context.RemoveContributionPayments
                            .Include(p => p.RemoveContribution)
                                .ThenInclude(c => c.AccountPerson)
                            .Include(p => p.PaymentAccount)
                            .Where(p => p.RemoveContribution.AccountId == accountId)
                            .Where(p => filter.PaymentAccountId == null || p.PaymentAccountId == filter.PaymentAccountId)
                            .Where(p => filter.FromDate == null || p.Date >= filter.FromDate)
                            .Where(p => filter.ToDate == null || p.Date <= filter.ToDate)
                            .Select(p => new PaymentHistoryDto
                            {
                                Id = p.Id ?? 0,
                                PaymentAccountName = p.PaymentAccount != null ? p.PaymentAccount.MethodName ?? "" : "",
                                AccountPersonName = p.RemoveContribution.AccountPerson != null ? p.RemoveContribution.AccountPerson.Name : "",
                                Description = p.RemoveContribution.Description ?? "",
                                Direction = "Paid",
                                Amount = p.Amount ?? 0,
                                Date = p.Date ?? p.RemoveContribution.Date ?? default,
                                PaymentType = "Remove Contribution",
                                BillNo = null
                            })
                            .ToListAsync();
                        history.AddRange(payments);
                    }
                }

                // 6. Payment Transfers
                var transfersQuery = _context.PaymentTransfers
                    .Include(t => t.FromPaymentAccount)
                    .Include(t => t.ToPaymentAccount)
                    .Where(t => t.AccountId == accountId)
                    .Where(t => filter.FromDate == null || t.Date >= filter.FromDate)
                    .Where(t => filter.ToDate == null || t.Date <= filter.ToDate)
                    .AsQueryable();

                if (filter.PaymentAccountId != null)
                {
                    transfersQuery = transfersQuery.Where(t => t.FromPaymentAccountId == filter.PaymentAccountId || t.ToPaymentAccountId == filter.PaymentAccountId);
                }

                var transfers = await transfersQuery.ToListAsync();

                foreach (var transfer in transfers)
                {
                    if (filter.PaymentAccountId == null || transfer.FromPaymentAccountId == filter.PaymentAccountId)
                    {
                        if (string.IsNullOrEmpty(filter.Direction) || filter.Direction == "Paid")
                        {
                            if (string.IsNullOrEmpty(filter.PaymentType) || filter.PaymentType == "Transfer")
                            {
                                history.Add(new PaymentHistoryDto
                                {
                                    Id = transfer.Id ?? 0,
                                    PaymentAccountName = transfer.FromPaymentAccount?.MethodName ?? "",
                                    Description = $"Transferred from {transfer.FromPaymentAccount?.MethodName} to {transfer.ToPaymentAccount?.MethodName}",
                                    Direction = "Paid",
                                    Amount = transfer.Amount ?? 0,
                                    Date = transfer.Date ?? default,
                                    PaymentType = "Transfer",
                                });
                            }
                        }
                    }

                    if (filter.PaymentAccountId == null || transfer.ToPaymentAccountId == filter.PaymentAccountId)
                    {
                        if (string.IsNullOrEmpty(filter.Direction) || filter.Direction == "Received")
                        {
                            if (string.IsNullOrEmpty(filter.PaymentType) || filter.PaymentType == "Transfer")
                            {
                                history.Add(new PaymentHistoryDto
                                {
                                    Id = transfer.Id ?? 0,
                                    PaymentAccountName = transfer.ToPaymentAccount?.MethodName ?? "",
                                    Description = $"Transferred from {transfer.FromPaymentAccount?.MethodName} to {transfer.ToPaymentAccount?.MethodName}",
                                    Direction = "Received",
                                    Amount = transfer.Amount ?? 0,
                                    Date = transfer.Date ?? default,
                                    PaymentType = "Transfer",
                                });
                            }
                        }
                    }
                }

                return history.OrderByDescending(h => h.Date).ThenByDescending(h => h.Id).ToList();
            });
        }

        public async Task<ServiceResponse<int>> CreatePaymentTransfer(CreatePaymentTransferDto dto)
        {
            return await ServiceResponse<int>.Execute(async err =>
            {
                var transfer = _mapper.Map<PaymentTransfer>(dto);
                transfer.AccountId = Token.AccountId;

                _context.PaymentTransfers.Add(transfer);
                await _context.SaveChangesAsync();

                return transfer.Id ?? default;
            });
        }

        public async Task<ServiceResponse<bool>> UpdatePaymentTransfer(UpdatePaymentTransferDto dto)
        {
            return await ServiceResponse<bool>.Execute(async err =>
            {
                var transfer = await _context.PaymentTransfers
                    .FirstOrDefaultAsync(t => t.Id == dto.Id && t.AccountId == Token.AccountId);

                if (transfer == null)
                {
                    err.AddError("Transfer not found");
                    return false;
                }

                _mapper.Map(dto, transfer);
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public async Task<ServiceResponse<bool>> DeletePaymentTransfer(int id)
        {
            return await ServiceResponse<bool>.Execute(async err =>
            {
                var transfer = await _context.PaymentTransfers
                    .FirstOrDefaultAsync(t => t.Id == id && t.AccountId == Token.AccountId);

                if (transfer == null)
                {
                    err.AddError("Transfer not found");
                    return false;
                }

                _context.PaymentTransfers.Remove(transfer);
                await _context.SaveChangesAsync();

                return true;
            });
        }

        public async Task<ServiceResponse<List<PaymentTransferDto>>> GetPaymentTransfers(PaymentTransferFilterDto filter)
        {
            return await ServiceResponse<List<PaymentTransferDto>>.Execute(async err =>
            {
                var query = _context.PaymentTransfers
                    .Include(t => t.FromPaymentAccount)
                    .Include(t => t.ToPaymentAccount)
                    .Where(t => t.AccountId == Token.AccountId)
                    .AsQueryable();

                if (filter.FromPaymentAccountId != null)
                    query = query.Where(t => t.FromPaymentAccountId == filter.FromPaymentAccountId);

                if (filter.ToPaymentAccountId != null)
                    query = query.Where(t => t.ToPaymentAccountId == filter.ToPaymentAccountId);

                if (filter.FromDate != null)
                    query = query.Where(t => t.Date >= filter.FromDate);

                if (filter.ToDate != null)
                    query = query.Where(t => t.Date <= filter.ToDate);

                var transfers = await query.OrderByDescending(t => t.Date).ToListAsync();
                return _mapper.Map<List<PaymentTransferDto>>(transfers);
            });
        }

        public async Task<ServiceResponse<PaymentTransferDto>> GetTransferById(int id)
        {
            return await ServiceResponse<PaymentTransferDto>.Execute(async err =>
            {
                var transfer = await _context.PaymentTransfers
                    .Include(t => t.FromPaymentAccount)
                    .Include(t => t.ToPaymentAccount)
                    .FirstOrDefaultAsync(t => t.Id == id && t.AccountId == Token.AccountId);

                if (transfer == null)
                {
                    err.AddError("Transfer not found");
                    return default;
                }

                return _mapper.Map<PaymentTransferDto>(transfer);
            });
        }
    }
}
