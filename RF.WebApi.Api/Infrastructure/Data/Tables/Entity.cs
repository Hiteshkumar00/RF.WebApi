namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class Entity
    {
        public int? Id { get; set; }
        public string? EntityName { get; set; }
        public string? DisplayName { get; set; }

        public virtual ICollection<RelatedEntity> RelatedEntities { get; set; } = new List<RelatedEntity>();
    }
}