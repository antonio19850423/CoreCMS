using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ExcelRowContext
    {
        /// <summary>
        /// شماره ردیف واقعی در Excel (مثلاً 3، 4، 5)
        /// </summary>
        public int ExcelRowNumber { get; set; }

        /// <summary>
        /// ایندکس ردیف در DataTable (۰-based)
        /// </summary>
        public int DataTableRowIndex { get; set; }
    }





}
