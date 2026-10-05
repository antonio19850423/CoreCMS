using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface INewsletterSubscriberService : IGenericService<SqlNewsletterSubscriber, SqlNewsletterSubscriber, NewsletterSubscriberDto>, IBaseService
    {
        Task<IQueryable<NewsletterSubscriberCrud>> GetAllViews();
        Task<ResultDto<NewsletterSubscriberDto>> CreateAsync(NewsletterSubscriberCrud input);
        Task<ResultDto<NewsletterSubscriberDto>> UpdateAsync(NewsletterSubscriberCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<ResultDto<NewsletterSubscriberDto>> SubscribeAsync(string email);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
