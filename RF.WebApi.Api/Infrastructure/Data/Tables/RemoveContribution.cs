namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class RemoveContribution
    {
        public int? Id { get; set; }
        public int? AccountPersonId { get; set; }
        public string? Description { get; set; }
        public DateOnly? Date { get; set; }

        public virtual AccountPerson? AccountPerson { get; set; }
        public virtual ICollection<RemoveContributionPayment> Payments { get; set; } = new List<RemoveContributionPayment>();
    }
}