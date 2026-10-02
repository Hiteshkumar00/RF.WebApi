using System.Collections.Generic;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IExcelService
    {
        byte[] Export<T>(IEnumerable<T> data, string sheetName = "Sheet1");
    }
}
