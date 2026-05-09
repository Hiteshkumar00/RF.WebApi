using System;
using System.Collections.Generic;

namespace RF.WebApi.Api.Application.DTOs.BusinessExpence
{
    public class BusinessExpenceDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string ExpenceType { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public decimal TotalAmount { get; set; }

        // Buying-bill linkage (null for direct expenses)
        public int? BuyingBillId { get; set; }
        public string? BuyingBillNo { get; set; }
        public string? AgencyName { get; set; }

        public List<BusinessExpencePaymentDto> Payments { get; set; } = new List<BusinessExpencePaymentDto>();
    }
}
