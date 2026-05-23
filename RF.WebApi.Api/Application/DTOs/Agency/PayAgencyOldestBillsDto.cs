namespace RF.WebApi.Api.Application.DTOs.Agency
{
    public class PayAgencyOldestBillsDto
    {
        public int AgencyId { get; set; }
        public List<AgencyPaymentEntryDto> Payments { get; set; } = new();
    }

    public class AgencyPaymentEntryDto
    {
        public decimal Amount { get; set; }
        public int PaymentAccountId { get; set; }
        public DateOnly Date { get; set; }
    }
}
