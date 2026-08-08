using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IProductReviewService : IGenericService<SqlProductReview, SqlProductReview, ProductReviewDto>, IBaseService
    {
        Task<IQueryable<ProductReviewCrud>> GetAllViews();
        Task<ResultDto<ProductReviewDto>> CreateAsync(ProductReviewCrud input);
        Task<ResultDto<ProductReviewDto>> UpdateAsync(ProductReviewCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<ResultDto<ProductReviewDto>> CreateUserReviewAsync(
    CreateProductReviewDto input);
        Task<bool> HasRecentReviewAsync(Guid productId);
        Task<ResultDto<ProductReviewListResultDto>> GetUserReviewsAsync(
            Guid productId,
            int page,
            int pageSize);

        Task<ResultDto<ProductRatingSummaryDto>> GetRatingSummaryAsync(
    Guid productId);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
