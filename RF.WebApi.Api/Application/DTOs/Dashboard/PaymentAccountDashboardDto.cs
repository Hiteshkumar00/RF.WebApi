using System.Collections.Generic;

namespace RF.WebApi.Api.Application.DTOs.Dashboard
{
    public class PaymentAccountDashboardDto
    {
        public decimal TotalAvailableBalance { get; set; }
        public List<PaymentAccountBalanceDto> PaymentAccountBalances { get; set; } = new();
    }
}
