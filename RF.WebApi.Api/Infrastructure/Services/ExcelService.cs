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
        public byte[] Export<T>(IEnumerable<T> data, string sheetName = "Sheet1", Dictionary<string, string> columnMapping = null)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            var allProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                      .Where(p => p.CanRead)
                                      .ToList();

            var propertiesToExport = allProperties;
            if (columnMapping != null && columnMapping.Any())
            {
                propertiesToExport = allProperties
                    .Where(p => columnMapping.ContainsKey(p.Name) || columnMapping.ContainsKey(char.ToLower(p.Name[0]) + p.Name.Substring(1)))
                    .ToList();
            }

            // Headers
            for (int i = 0; i < propertiesToExport.Count; i++)
            {
                var propName = propertiesToExport[i].Name;
                var camelCasePropName = char.ToLower(propName[0]) + propName.Substring(1);

                string headerName = propName;
                if (columnMapping != null)
                {
                    if (columnMapping.ContainsKey(propName))
                        headerName = columnMapping[propName];
                    else if (columnMapping.ContainsKey(camelCasePropName))
                        headerName = columnMapping[camelCasePropName];
                }

                worksheet.Cell(1, i + 1).Value = headerName;
            }

            // Data
            int rowIndex = 2;
            foreach (var item in data)
            {
                for (int i = 0; i < propertiesToExport.Count; i++)
                {
                    var value = propertiesToExport[i].GetValue(item);
                    worksheet.Cell(rowIndex, i + 1).Value = value != null ? value.ToString() : string.Empty;
                }
                rowIndex++;
            }

            // Style headers
            if (propertiesToExport.Count > 0)
            {
                var headerRange = worksheet.Range(1, 1, 1, propertiesToExport.Count);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#4F81BD"); // Blue header
                headerRange.SetAutoFilter();
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
