using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Infrastructure.ORM.Repositories.EfCore
{
    public class EfModel
    {
        public Guid Key;
        public DbContext dbContext;
        public bool dispose;
        public DateTime CreateDate;
        public DateTime LastAccessed;
    }

}
