using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Enums
{
    public static class DataTableErrorExtensions
    {
        public static void SetRowError(
            this DataTable table,
            int rowIndex,
            string errorMessage)
        {
            table.Rows[rowIndex]["Error"] = errorMessage;
        }
    }

}
