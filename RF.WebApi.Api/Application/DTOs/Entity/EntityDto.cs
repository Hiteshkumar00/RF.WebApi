using RF.WebApi.Api.Application.DTOs.RelatedEntity;

namespace RF.WebApi.Api.Application.DTOs.Entity
{
    public class EntityDto
    {
        public int? Id { get; set; }
        public string? EntityName { get; set; }
        public string? DisplayName { get; set; }
        
        public List<RelatedEntityDto> RelatedEntities { get; set; } = new();
    }
}
