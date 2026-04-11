namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class AddContribution
    {
        public int? Id { get; set; }
        public int? AccountPersonId { get; set; }
        public int? AccountId { get; set; }
        public string? Description { get; set; }
        public DateOnly? Date { get; set; }

        public virtual AccountPerson? AccountPerson { get; set; }
        public virtual ICollection<AddContributionPayment> Payments { get; set; } = new List<AddContributionPayment>();
    }
}