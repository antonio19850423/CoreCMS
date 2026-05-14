using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Services
{
    public interface ITransactionService:IBaseService
    {
        Task CommitAsync();
        Task RollbackAsync();
    }

}
