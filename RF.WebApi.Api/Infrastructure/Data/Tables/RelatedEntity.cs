namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class RelatedEntity
    {
        public int? Id { get; set; }
        public string? RelatedEntityName { get; set; }
        public string? RelatedDisplayName { get; set; }
        public int? EntityId { get; set; } 

        public virtual Entity Entity { get; set; }
    }
}