using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;
using Velora.Infrastructure.ORM.Repositories.EfCore;

namespace MyApp.Orm.EfCore
    {
    public class EfUnitOfWork<TContext>:IUnitOfWork where TContext : DbContext, new()
        {
        private readonly Guid _contextKey;
        private bool _disposed = false;
        private IDbContextTransaction? _transaction;
        private readonly TContext _context;
        private readonly bool _useTransaction;

        public EfUnitOfWork(bool useTransaction = true)
            {
            _contextKey = Guid.NewGuid();
            _context = ContextQueue<TContext>.GetContext(_contextKey);
            _useTransaction = useTransaction;

            if(_useTransaction)
                _transaction = _context.Database.BeginTransaction();
            }
        public EfUnitOfWork(TContext context, bool useTransaction = true)
        {
            _contextKey = Guid.NewGuid();
            _context = context;
            _useTransaction = useTransaction;

            if (_useTransaction)
                _transaction = _context.Database.BeginTransaction();
        }

        public Guid ContextKey => _contextKey;
        public DbContext Context => _context;

        public async Task<int> CommitAsync()
            {
            try
                {
                int result = _context.SaveChanges();
                if(_useTransaction && _transaction != null)
                    await _transaction.CommitAsync();
                await _transaction.DisposeAsync();

                // ✅ ایجاد تراکنش جدید برای ادامه عملیات
                _transaction = await _context.Database.BeginTransactionAsync();
                return result;
                }
            catch
                {
                if(_useTransaction)
                    await RollbackAsync();
                throw;
                }
            }

        public async Task RollbackAsync()
            {
            if(_useTransaction && _transaction != null)
                await _transaction.RollbackAsync();
            }

        public void Rollback()
            {
            if(_useTransaction)
                _transaction?.Rollback();
            }

        public void Dispose()
            {
            if(!_disposed)
                {
                try
                    {
                    if(_useTransaction)
                        _transaction?.Dispose();
                    }
                catch { }

                ContextQueue<TContext>.Dispose(_contextKey);
                _disposed = true;
                }
            }
        }


    }
