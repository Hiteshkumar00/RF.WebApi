using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.RelatedEntity
{
    public class CreateRelatedEntityDto
    {
        [Required(ErrorMessage = "Related Entity Name is required.")]
        [StringLength(250)]
        public string? RelatedEntityName { get; set; }
        
        [StringLength(250)]
        public string? RelatedDisplayName { get; set; }
    }
}
