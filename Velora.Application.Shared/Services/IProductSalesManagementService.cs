using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IProductSalesManagementService : IGenericService<SqlProductSalesManagement, SqlProductSalesManagement, SqlProductSalesManagement>, IBaseService
    {
        Task<IQueryable<ProductSalesManagementCrud>> GetAllViews();
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
