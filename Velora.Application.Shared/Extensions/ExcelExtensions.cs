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
    public static class ExcelExtensions
    {
        public static (DataTable Table, List<ExcelRowContext> Rows)
            LoadExcelWithErrors(
                this Stream excelStream,
                int headerRowIndex = 1,
                int dataStartRowIndex = 3,
                int shouldInsertColumnIndex = 1
            )
        {
            var dt = new DataTable();
            var rowContexts = new List<ExcelRowContext>();

            using var workbook = new XLWorkbook(excelStream);
            var sheet = workbook.Worksheets.First();

            // 1️⃣ Header
            var headerRow = sheet.Row(headerRowIndex);
            foreach (var cell in headerRow.CellsUsed())
            {
                var colName = cell.GetString().Trim();
                if (!dt.Columns.Contains(colName))
                    dt.Columns.Add(colName);
            }

            // 2️⃣ Rows
            for (int r = dataStartRowIndex; r <= sheet.LastRowUsed().RowNumber(); r++)
            {
                var row = sheet.Row(r);

                var shouldInsertCell = row.Cell(shouldInsertColumnIndex);
                var shouldInsert =
                    !shouldInsertCell.IsEmpty() &&
                    (shouldInsertCell.GetString().Trim().ToUpper() == "TRUE" ||
                     shouldInsertCell.GetString().Trim() == "1");

                if (!shouldInsert)
                    continue;

                var dataRow = dt.NewRow();
                int dataTableRowIndex = dt.Rows.Count;

                foreach (DataColumn col in dt.Columns)
                {
                    var cell = row.Cell(col.Ordinal + 1);
                    dataRow[col.ColumnName] =
                        cell.IsEmpty() ? DBNull.Value : cell.Value.ToString();
                }

                dt.Rows.Add(dataRow);

                rowContexts.Add(new ExcelRowContext
                {
                    ExcelRowNumber = r,                 // شماره ردیف اکسل
                    DataTableRowIndex = dataTableRowIndex // ایندکس دیتاتیبل
                });
            }

            return (dt, rowContexts);
        }
        /// <summary>
        /// پر کردن داده‌ها در یک Worksheet با استفاده از Row اول به عنوان ستون‌ها
        /// </summary>
        public static void FillData<T>(this IXLWorksheet sheet, IList<T> data, int startRow = 2)
        {
            // خواندن ستون‌ها از ردیف اول
            var columns = sheet.Row(1)
                .CellsUsed()
                .Select(c => new
                {
                    ColumnIndex = c.Address.ColumnNumber,
                    PropertyName = c.GetString()
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.PropertyName))
                .ToList();

            int rowIndex = startRow;

            foreach (var item in data)
            {
                foreach (var col in columns)
                {
                    var prop = typeof(T).GetProperty(col.PropertyName);
                    if (prop == null)
                        continue;

                    var value = prop.GetValue(item);
                    var cell = sheet.Cell(rowIndex, col.ColumnIndex);

                    if (value == null)
                    {
                        cell.SetValue(string.Empty);
                        continue;
                    }

                    // تبدیل صریح به نوع مناسب ClosedXML
                    switch (value)
                    {
                        case DateTimeOffset dto: cell.SetValue(dto.DateTime); break;
                        case DateTime dt: cell.SetValue(dt); break;
                        case Enum e: cell.SetValue(e.ToString()); break;
                        case Guid g: cell.SetValue(g.ToString()); break;
                        case decimal m: cell.SetValue((double)m); break;
                        case byte[] bytes: cell.SetValue(Convert.ToBase64String(bytes)); break;
                        case int i: cell.SetValue(i); break;
                        case double d: cell.SetValue(d); break;
                        case float f: cell.SetValue((double)f); break;
                        case bool b: cell.SetValue(b); break;
                        default: cell.SetValue(value.ToString()); break;
                    }
                }

                rowIndex++;
            }

            sheet.Columns().AdjustToContents();
        }

        /// <summary>
        /// پر کردن داده‌ها با استفاده از یک Template
        /// </summary>
        public static byte[] FillDataIntoTemplate<T>(this byte[] templateBytes, IList<T> data, int startRow = 2)
        {
            using var inputStream = new MemoryStream(templateBytes);
            using var workbook = new XLWorkbook(inputStream);

            var sheet = workbook.Worksheets.First();
            sheet.FillData(data, startRow);

            using var outputStream = new MemoryStream();
            workbook.SaveAs(outputStream);
            return outputStream.ToArray();
        }
    }
}
