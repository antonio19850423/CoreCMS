using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ICityService : IGenericService<SqlCity, SqlCity, CityDto>, IBaseService
    {
        Task<IQueryable<CityCrud>> GetAllViews();
        Task<ResultDto<CityDto>> CreateAsync(CityCrud input);
        Task<ResultDto<CityDto>> UpdateAsync(CityCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
        Task<ResultDto<List<CityDto>>> GetCitiesByStateIdAsync(
    Guid stateId);
    }
}
