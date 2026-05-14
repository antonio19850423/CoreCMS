using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Repositories;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

public class EfCoreRepository<TEntity> : ISqlRepository<TEntity> , IPosgreSqlRepository<TEntity> where TEntity : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public EfCoreRepository(IUnitOfWork unitOfWork)
    {
        _context = unitOfWork.Context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<IQueryable<TEntity>> GetAllAsync()
    {
        return _dbSet.AsNoTracking();
    }

    public async Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var query = _dbSet.AsNoTracking();

        if (predicate != null)
            query = query.Where(predicate);

        return query;
    }

    public IQueryable<TEntity> GetAllQueryable()
    {
        return _dbSet.AsNoTracking();
    }
    public IQueryable<TView> GetAllViewQueryable<TView>() where TView : class
    {
        return _context.Set<TView>().AsNoTracking();
    }
    public async Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public async Task<List<TResult>> GetListAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        params Expression<Func<TEntity, object>>[] includes) where TResult : class
    {
        var query = _dbSet.AsNoTracking();

        if (includes != null)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        // ابتدا داده‌ها را در حافظه لود کنید
        var entities = await query.Where(predicate).ToListAsync();

        // projection امن در حافظه
        return entities
            .Select(selector.Compile())  // Compile selector برای اجرا در حافظه
            .ToList();
    }

    public async Task<TEntity> GetByIdAsync(params object[] keyValues)
    {
        return await _dbSet.FindAsync(keyValues);
    }
    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return true;
    }

    public async Task<bool> RemoveAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null) return false;
        _dbSet.Remove(entity);
        return true;
    }

    public async Task<bool> DeleteRangeAsync(IList<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
        return true;
    }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task ExecuteSqlAsync(string sql, params object[] parameters)
    {
        await _context.Database.ExecuteSqlRawAsync(sql, parameters);
    }
    public async Task<bool> AnyRelatedAsync<TEntity>(Guid id) where TEntity : class
    {
        var entityType = _context.Model.FindEntityType(typeof(TEntity));
        if (entityType == null) return false;

        var foreignKeys = entityType.GetForeignKeys();

        foreach (var fk in foreignKeys)
        {
            var dependentType = fk.DeclaringEntityType.ClrType;

            // دریافت DbSet داینامیک با Reflection
            var method = typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes)
                                           .MakeGenericMethod(dependentType);
            var dbSet = method.Invoke(_context, null);

            // تبدیل به IQueryable<dynamic>
            var queryable = dbSet as IQueryable;
            if (queryable == null) continue;

            foreach (var property in fk.Properties)
            {
                var columnName = property.Name;

                // ساخت Expression داینامیک با EF.Property
                var parameter = Expression.Parameter(dependentType, "e");
                var propertyAccess = Expression.Call(
                    typeof(EF),
                    nameof(EF.Property),
                    new Type[] { typeof(Guid) },
                    parameter,
                    Expression.Constant(columnName)
                );
                var equals = Expression.Equal(propertyAccess, Expression.Constant(id));
                var lambda = Expression.Lambda(equals, parameter);

                // فراخوانی Where و AnyAsync به صورت داینامیک
                var whereMethod = typeof(Queryable).GetMethods()
                    .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(dependentType);
                var filteredQuery = whereMethod.Invoke(null, new object[] { queryable, lambda });

                var anyAsyncMethod = typeof(EntityFrameworkQueryableExtensions)
                    .GetMethods()
                    .First(m => m.Name == "AnyAsync" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(dependentType);

                var task = (Task<bool>)anyAsyncMethod.Invoke(null, new object[] { filteredQuery, CancellationToken.None });
                if (await task)
                    return true;
            }
        }

        return false;
    }

    public async Task<IList<T>> ExecuteSqlQueryAsync<T>(string sql, params object[] parameters) where T : class
    {
        return await _context.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
    }

    public async Task<T> ExecuteScalarAsync<T>(string sql, params object[] parameters)
    {
        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        if (command.Connection.State != System.Data.ConnectionState.Open)
            await command.Connection.OpenAsync();

        for (int i = 0; i < parameters.Length; i++)
        {
            var param = command.CreateParameter();
            param.ParameterName = $"@p{i}";
            param.Value = parameters[i] ?? DBNull.Value;
            command.Parameters.Add(param);
        }

        var result = await command.ExecuteScalarAsync();
        return (T)Convert.ChangeType(result, typeof(T));
    }

    public IQueryable<TView> GetViewQueryable<TView>() where TView : class
    {
        return _context.Set<TView>().AsNoTracking();
    }

    public async Task<(IList<TView> Data, int Count)> GetViewWithODataAsync<TView>(ODataQueryOptions<TView> queryOptions) where TView : class
    {
        var query = _context.Set<TView>().AsQueryable();

        var filtered = (IQueryable<TView>)queryOptions.ApplyTo(query);

        var count = await Task.Run(() => filtered.Count());

        var data = await Task.Run(() => filtered.ToList());

        return (data, count);
    }

    public async Task<bool> UpdateAttachAsync(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return true;
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        if (predicate == null)
            return await _dbSet.CountAsync();
        return await _dbSet.CountAsync(predicate);
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public IQueryable<TEntity> ReadOnly()
    {
        return _dbSet.AsNoTracking();
    }




    //public void Dispose()
    //{
    //    _context?.Dispose();
    //}



}
