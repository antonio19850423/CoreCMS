using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ICustomerManagementService : IGenericService<SqlCustomerManagement, SqlCustomerManagement, SqlCustomerManagement>, IBaseService
    {
        Task<IQueryable<CustomerManagementCrud>> GetAllViews();
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
