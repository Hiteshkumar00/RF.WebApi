using System.Collections.Generic;

namespace RF.WebApi.Api.Application.DTOs.Common
{
    public class PagedResult<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int TotalRecords { get; set; }
    }
}
