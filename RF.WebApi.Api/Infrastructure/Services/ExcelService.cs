using ClosedXML.Excel;
using RF.WebApi.Api.Domain.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class ExcelService : IExcelService
    {
        public byte[] Export<T>(IEnumerable<T> data, string sheetName = "Sheet1")
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                      .Where(p => p.CanRead)
                                      .ToList();

            // Headers
            for (int i = 0; i < properties.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
            }

            // Data
            int rowIndex = 2;
            foreach (var item in data)
            {
                for (int i = 0; i < properties.Count; i++)
                {
                    var value = properties[i].GetValue(item);
                    worksheet.Cell(rowIndex, i + 1).Value = value != null ? value.ToString() : string.Empty;
                }
                rowIndex++;
            }

            // Style headers
            var headerRange = worksheet.Range(1, 1, 1, properties.Count);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
