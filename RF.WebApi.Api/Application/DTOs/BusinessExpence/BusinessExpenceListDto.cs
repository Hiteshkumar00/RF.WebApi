using System;

namespace RF.WebApi.Api.Application.DTOs.BusinessExpence
{
    public class BusinessExpenceListDto
    {
        public int Id { get; set; }
        public string ExpenceType { get; set; } = string.Empty;
        public DateOnly Date { get; set; }

        // Expense financials
        public decimal TotalAmount { get; set; }   // Declared expense total
        public decimal PaidAmount { get; set; }    // Sum of all payment entries
        public decimal RemainingAmount { get; set; } // TotalAmount - PaidAmount

        // Buying-bill linkage (null = direct expense)
        public int? BuyingBillId { get; set; }
        public string? BuyingBillNo { get; set; }
        public string? AgencyName { get; set; }
    }
}
