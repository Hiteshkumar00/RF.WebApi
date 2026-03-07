using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Application.DTOs.RelatedEntity;

namespace RF.WebApi.Api.Application.DTOs.Entity
{
    public class CreateEntityDto
    {
        [Required(ErrorMessage = "Entity Name is required.")]
        [StringLength(250)]
        public string? EntityName { get; set; }
        
        [StringLength(250)]
        public string? DisplayName { get; set; }

        public List<CreateRelatedEntityDto>? RelatedEntities { get; set; }
    }
}
