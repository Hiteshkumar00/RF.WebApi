using System;

namespace RF.WebApi.Api.Application.DTOs.BusinessExpence
{
    public class BusinessExpenceListDto
    {
        public int Id { get; set; }
        public string ExpenceType { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        
        // Summarized amount instead of sending all payments
        public decimal PaidAmount { get; set; }
    }
}
