using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
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
                foreach (var entry in _context.ChangeTracker.Entries())
                {
                    Console.WriteLine(
                        $"ENTITY: {entry.Entity.GetType().Name} | STATE: {entry.State}");

                    if (entry.Entity is Product product)
                    {
                        Console.WriteLine(
                            $"PRODUCT => Id={product.Id}, Name='{product.Name}', Slug='{product.Slug}'");
                    }
                }

                int result = _context.SaveChanges();

                if (_useTransaction && _transaction != null)
                {
                    await _transaction.CommitAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }

                return result;
            }
            catch
            {
                if (_useTransaction && _transaction != null)
                    await RollbackAsync();

                throw;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null)
                return;

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
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
