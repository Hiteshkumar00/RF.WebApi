using System;

namespace RF.WebApi.Api.Application.DTOs.AgencyPayment
{
    public class AgencyPaymentListDto
    {
        public int Id { get; set; }
        public int AgencyId { get; set; }
        public string AgencyName { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public string? Description { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
