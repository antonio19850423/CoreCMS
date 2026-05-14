using ClosedXML.Excel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Extensions
{
    public static class ExcelErrorExportExtensions
    {
        public static string SaveErrorExcel(
            this DataTable table,
            string webRootPath,
            IConfiguration config)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Result");

            // Header
            for (int c = 0; c < table.Columns.Count; c++)
                sheet.Cell(1, c + 1).Value = table.Columns[c].ColumnName;

            // Data
            for (int r = 0; r < table.Rows.Count; r++)
            {
                for (int c = 0; c < table.Columns.Count; c++)
                    sheet.Cell(r + 2, c + 1).Value =
                        table.Rows[r][c]?.ToString() ?? "";

                // رنگ سطرهای خطادار
                if (!string.IsNullOrWhiteSpace(table.Rows[r]["Error"]?.ToString()))
                {
                    sheet.Row(r + 2)
                        .Style.Fill.BackgroundColor = XLColor.LightPink;
                }
            }

            sheet.Columns().AdjustToContents();

            var root = webRootPath ??
                       System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var folder = System.IO.Path.Combine(root, "uploads", "excel");
            Directory.CreateDirectory(folder);

            var fileName = $"BulkInsert_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            var path = System.IO.Path.Combine(folder, fileName);
            workbook.SaveAs(path);

            var baseUrl = config["App:BaseUrl"]?.TrimEnd('/') ?? "";
            return $"{baseUrl}/uploads/excel/{fileName}";
        }
    }

}
