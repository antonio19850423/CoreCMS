using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Extensions
{
    public static class ExcelImportExtensions
    {
        public static (DataTable Table, List<ExcelRowContext> RowContexts)
            LoadExcelWithErrors(this Stream excelStream)
        {
            using var workbook = new XLWorkbook(excelStream);
            var worksheet = workbook.Worksheets.First();

            var dt = new DataTable();
            var rowContexts = new List<ExcelRowContext>();

            // Header (Row 1)
            // Header (Row 1) — شامل ستون‌های مخفی و نگه داشتن شماره ستون واقعی
            var columnMap = new Dictionary<string, int>(); // ColumnName -> ExcelColumnNumber

            for (int c = 1; c <= worksheet.LastColumnUsed().ColumnNumber(); c++)
            {
                var cell = worksheet.Cell(1, c);
                var columnName = cell.GetString().Trim();
                if (!string.IsNullOrEmpty(columnName) && !dt.Columns.Contains(columnName))
                {
                    dt.Columns.Add(columnName);
                    columnMap[columnName] = c; // شماره ستون واقعی Excel
                }
            }


            // ستون خطا
            if (!dt.Columns.Contains("Error"))
                dt.Columns.Add("Error");

            int dtRowIndex = 0;

            for (int r = 3; r <= worksheet.LastRowUsed().RowNumber(); r++)
            {
                var row = worksheet.Row(r);

                // ShouldInsert
                var shouldInsertCell = row.Cell(1);
                var shouldInsert =
                    !shouldInsertCell.IsEmpty() &&
                    shouldInsertCell.GetString().Trim().ToUpper() == "TRUE";

                if (!shouldInsert)
                    continue;

                var dataRow = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.ColumnName == "Error") continue;

                    var cell = row.Cell(col.Ordinal + 1);
                    dataRow[col.ColumnName] =
                        cell.IsEmpty() ? DBNull.Value : cell.Value.ToString();
                }

                dt.Rows.Add(dataRow);

                rowContexts.Add(new ExcelRowContext
                {
                    ExcelRowNumber = r,
                    DataTableRowIndex = dtRowIndex
                });

                dtRowIndex++;
            }

            return (dt, rowContexts);
        }
    }

}
