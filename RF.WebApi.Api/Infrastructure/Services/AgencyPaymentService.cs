using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.AgencyPayment;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class AgencyPaymentService : IAgencyPaymentService
    {
        private readonly RFDBContext _context;
        private readonly IMapper _mapper;

        public AgencyPaymentService(RFDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<int>> CreateAgencyPayment(CreateAgencyPaymentDto dto)
        {
            return await ServiceResponse<int>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var payment = _mapper.Map<AgencyPayment>(dto);
                payment.AccountId = accountId;

                _context.AgencyPayments.Add(payment);
                await _context.SaveChangesAsync();

                return payment.Id ?? default;
            });
        }

        public Task<ServiceResponse<AgencyPaymentDto>> GetAgencyPaymentById(int id)
        {
            return ServiceResponse<AgencyPaymentDto>.Execute(async err =>
            {
                var payment = await _context.AgencyPayments
                    .Include(ap => ap.Agency)
                    .Include(ap => ap.Transactions)
                        .ThenInclude(t => t.PaymentAccount)
                    .FirstOrDefaultAsync(ap => ap.Id == id && ap.AccountId == Token.AccountId);

                if (payment == null)
                {
                    err.AddError("Agency Payment not found");
                    return default;
                }

                return _mapper.Map<AgencyPaymentDto>(payment);
            });
        }

        public Task<ServiceResponse<bool>> UpdateAgencyPayment(UpdateAgencyPaymentDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var payment = await _context.AgencyPayments
                    .Include(ap => ap.Transactions)
                    .FirstOrDefaultAsync(ap => ap.Id == dto.Id && ap.AccountId == Token.AccountId);

                if (payment == null)
                {
                    err.AddError("Agency Payment not found");
                    return false;
                }

                var oldParentDate = payment.Date;

                _mapper.Map(dto, payment);

                _context.SyncCollection(payment.Transactions, dto.Transactions, (e, d) => d.Id > 0 && e.Id == d.Id, _mapper);

                foreach (var tx in payment.Transactions)
                {
                    tx.AgencyPaymentId = payment.Id;

                    if (tx.Date == oldParentDate)
                    {
                        tx.Date = null;
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<bool>> DeleteAgencyPayment(int id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var payment = await _context.AgencyPayments
                    .Include(ap => ap.Transactions)
                    .FirstOrDefaultAsync(ap => ap.Id == id && ap.AccountId == Token.AccountId);

                if (payment == null)
                {
                    err.AddError("Agency Payment not found");
                    return false;
                }

                _context.AgencyPaymentTransactions.RemoveRange(payment.Transactions);
                _context.AgencyPayments.Remove(payment);

                await _context.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<List<AgencyPaymentListDto>>> GetAllAgencyPayments()
        {
            return ServiceResponse<List<AgencyPaymentListDto>>.Execute(async err =>
            {
                var accountId = Token.AccountId;

                var payments = await _context.AgencyPayments
                    .Include(ap => ap.Agency)
                    .Include(ap => ap.Transactions)
                    .Where(ap => ap.AccountId == accountId)
                    .OrderByDescending(ap => ap.Date)
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<AgencyPaymentListDto>>(payments);
            });
        }
    }
}
