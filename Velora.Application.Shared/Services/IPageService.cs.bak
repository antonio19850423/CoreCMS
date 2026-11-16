using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IPageService : IGenericService<SqlPage, SqlPage, PageDto>, IBaseService
    {
        Task<IQueryable<PageCrud>> GetAllViews();
        Task<ResultDto<PageDto>> CreateAsync(PageCrud input);
        Task<ResultDto<PageDto>> UpdateAsync(PageCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
        Task<ResultDto<PageViewDto>> GetPageAsync(string slug);
        Task<ResultDto<FooterDto>> GetFooterAsync();
        Task<ResultDto<PageViewDto>> GetContentPageAsync(
            string slug,
            int page = 1,
            int pageSize = 10,
            string? categorySlug = null,
            string? search = null,
            string? contentType = null,
            string sort = "newest");
        Task<ResultDto<ContentItemListDto>> GetContentDetailAsync(
                    string contentType,
                    string slug);
    }
}
