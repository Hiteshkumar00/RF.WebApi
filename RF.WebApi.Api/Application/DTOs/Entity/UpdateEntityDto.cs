using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Application.DTOs.RelatedEntity;

namespace RF.WebApi.Api.Application.DTOs.Entity
{
    public class UpdateEntityDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Entity Name is required.")]
        [StringLength(250)]
        public string? EntityName { get; set; }
        
        [StringLength(250)]
        public string? DisplayName { get; set; }

        public List<UpdateRelatedEntityDto>? RelatedEntities { get; set; }
    }
}
