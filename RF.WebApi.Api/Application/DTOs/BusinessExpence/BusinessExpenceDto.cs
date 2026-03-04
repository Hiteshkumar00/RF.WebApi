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

        public List<BusinessExpencePaymentDto> Payments { get; set; } = new List<BusinessExpencePaymentDto>();
    }
}
