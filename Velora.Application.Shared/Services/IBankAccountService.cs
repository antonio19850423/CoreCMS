using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IBankAccountService : IGenericService<SqlBankAccount, SqlBankAccount, BankAccountDto>, IBaseService
    {
        Task<IQueryable<BankAccountCrud>> GetAllViews();
        Task<ResultDto<BankAccountDto>> CreateAsync(BankAccountCrud input);
        Task<ResultDto<BankAccountDto>> UpdateAsync(BankAccountCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<IQueryable<BankAccountCrud>> GetBankAccountsBySiteInfoId(Guid siteInfoId);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
