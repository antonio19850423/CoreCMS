using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{
    public class ContentService : IContentService
    {
        private readonly IViewQueryService _viewQueryService;

        public ContentService(IViewQueryService viewQueryService)
        {
            _viewQueryService = viewQueryService;
        }

        public async Task<ResultDto<SqlSiteGlobalSetting>> GetSiteInfoAsync()
        {
            try
            {
                var data = await _viewQueryService
                    .GetListAsync<SqlSiteGlobalSetting, SqlSiteGlobalSetting>();

                return new ResultDto<SqlSiteGlobalSetting>
                {
                    Data = data.FirstOrDefault(),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ResultDto<SqlSiteGlobalSetting>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
