using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Velora.Infrastructure.ORM.Repositories.EfCore;

public static class ContextQueue<TContext> where TContext : DbContext, new()
{
    private static readonly ConcurrentDictionary<Guid, EfModel> _contexts = new();

    private static readonly TimeSpan AutoDisposeDelay = TimeSpan.FromDays(1);

    public static TContext GetContext(Guid? key = null)
    {
        key ??= Guid.NewGuid();

        var efModel = _contexts.GetOrAdd(key.Value, k =>
        {
            var context = new TContext();
            return new EfModel
            {
                Key = k,
                dbContext = context,
                dispose = false,
                CreateDate = DateTime.Now,
                LastAccessed = DateTime.Now
            };
        });

        // بروزرسانی زمان آخرین استفاده
        efModel.LastAccessed = DateTime.Now;

        // شروع dispose خودکار بعد از تاخیر
        _ = DisposeLaterSafeAsync(key.Value, AutoDisposeDelay);

        return (TContext)efModel.dbContext;
    }

    private static async Task DisposeLaterSafeAsync(Guid key, TimeSpan delay)
    {
        await Task.Delay(delay);

        if (_contexts.TryGetValue(key, out var model))
        {
            // اگر از آخرین استفاده بیش از delay گذشته بود، Dispose کن
            if ((DateTime.Now - model.LastAccessed) >= delay)
            {
                if (_contexts.TryRemove(key, out _))
                {
                    try { model.dbContext?.Dispose(); } catch { }
                    model.dispose = true;
                }
            }
        }
    }

    /// <summary>
    /// Dispose دستی context
    /// </summary>
    public static void Dispose(Guid key)
    {
        if (_contexts.TryRemove(key, out var model))
        {
            try { model.dbContext?.Dispose(); } catch { }
            model.dispose = true;
        }
    }
}
