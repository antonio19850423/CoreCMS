using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
    {
    public interface IGenericService<TEntitySql, TEntityPosgreSql, TDto>
        where TEntitySql : class
        where TEntityPosgreSql : class
        where TDto : class
        {
        Task<ResultDto<IEnumerable<TDto>>> GetAllAsync();
        Task<IQueryable<TDto>> GetAllQuery();
        Task<IQueryable<TResult>> GetAllViewQueryable<PgView, SqlView, TResult>()
            where PgView : class
            where SqlView : class
            where TResult : class;
        Task<ResultDto<TDto?>> GetByIdAsync(params object[] idies);
        Task<ResultDto<TDto>> CreateAsync(TDto dto);
        Task<ResultDto<TDto?>> UpdateAsync<TDtoParam>(TDtoParam updatedDto,params object[] idies) where TDtoParam : class;
        Task<ResultDto<bool>> DeleteAsync(Guid id);
        Task<ResultDto<TDto?>> FirstOrDefaultAsync<TPredicateEntity>(Expression<Func<TPredicateEntity,bool>> predicate) where TPredicateEntity : class;
        }
    }
