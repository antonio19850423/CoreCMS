using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IOrderManagementService : IGenericService<SqlOrderManagement, SqlOrderManagement, SqlOrderManagement>, IBaseService
    {
        Task<IQueryable<OrderManagementCrud>> GetAllViews();
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
