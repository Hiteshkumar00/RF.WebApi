using System.Collections.Generic;

namespace RF.WebApi.Api.Application.DTOs.Common
{
    public class TableLazyLoadEventDto
    {
        public int? First { get; set; } = 0;
        public int? Rows { get; set; } = 10;
        public string? SortField { get; set; }
        public int? SortOrder { get; set; } // 1 for Ascending, -1 for Descending
        public Dictionary<string, FilterMetadataDto[]>? Filters { get; set; }
        public string? GlobalFilter { get; set; } // For the global search term
    }
}
