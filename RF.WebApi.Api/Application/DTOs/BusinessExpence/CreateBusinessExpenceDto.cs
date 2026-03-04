using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.BusinessExpence
{
    public class CreateBusinessExpenceDto
    {
        [Required(ErrorMessage = BusinessExpenceMessages.ExpenceTypeRequired)]
        [StringLength(250)]
        public string ExpenceType { get; set; } = string.Empty;

        [Required(ErrorMessage = BusinessExpenceMessages.DateRequired)]
        public DateOnly Date { get; set; }

        public List<CreateBusinessExpencePaymentDto> Payments { get; set; } = new List<CreateBusinessExpencePaymentDto>();
    }
}
