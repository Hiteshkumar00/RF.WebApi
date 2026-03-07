using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.RelatedEntity
{
    public class UpdateRelatedEntityDto
    {
        public int? Id { get; set; } // Nullable because it can be an insert on an existing entity

        [Required(ErrorMessage = "Related Entity Name is required.")]
        [StringLength(250)]
        public string? RelatedEntityName { get; set; }
        
        [StringLength(250)]
        public string? RelatedDisplayName { get; set; }
    }
}
