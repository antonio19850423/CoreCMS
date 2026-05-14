using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Infrastructure.ORM.Interfaces
{
    using System;
    using System.Threading.Tasks;

    namespace MyApp.Orm.Interfaces
    {
        public interface IUnitOfWork : IDisposable
        {
            /// <summary>
            /// Commit all changes to the database.
            /// </summary>
            Task<int> CommitAsync();

            /// <summary>
            /// Rollback the current transaction.
            /// </summary>
            Task RollbackAsync();

            /// <summary>
            /// Optionally get the DbContext if needed.
            /// </summary>
            DbContext Context { get; }
        }

    }

}
