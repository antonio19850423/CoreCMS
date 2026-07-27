using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{
    public class GenericService<TEntitySql, TEntityPosgreSql, TDto> : IGenericService<TEntitySql, TEntityPosgreSql, TDto>
        where TEntitySql : class
        where TEntityPosgreSql : class
        where TDto : class
    {
        protected readonly ISqlRepository<TEntitySql> _sqlRepository;
        protected readonly IPosgreSqlRepository<TEntityPosgreSql> _pgRepository;
        protected readonly IMapper _mapper;
        protected readonly DatabaseType _dbType;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        protected readonly ICurrentUserService _currentUserService;
        public GenericService(
            ISqlRepository<TEntitySql> sqlRepository,
            IPosgreSqlRepository<TEntityPosgreSql> pgRepository,
            IMapper mapper,
            IConfiguration configuration,
            Lazy<ILocalizationMessageService> messageService,
            ICurrentUserService currentUserService)
        {
            _sqlRepository = sqlRepository;
            _pgRepository = pgRepository;
            _mapper = mapper;
            _messageService = messageService;
            _currentUserService = currentUserService;
            var dbTypeString = configuration.GetValue<string>("Database:Provider") ?? "PostgreSql";
            _dbType = dbTypeString.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)
                ? DatabaseType.SqlServer
                : DatabaseType.PostgreSql;
        }

        protected dynamic GetRepository() => _dbType == DatabaseType.SqlServer ? _sqlRepository : _pgRepository;
        protected ISqlRepository<TEntitySql> GetSqlRepository() => _dbType == DatabaseType.SqlServer ? _sqlRepository : throw new InvalidOperationException("Active DB is not SQL Server");
        protected IPosgreSqlRepository<TEntityPosgreSql> GetPgRepository() => _dbType == DatabaseType.PostgreSql ? _pgRepository : throw new InvalidOperationException("Active DB is not PostgreSQL");

        // 🔹 GetAllAsync
        public async Task<ResultDto<IEnumerable<TDto>>> GetAllAsync()
        {
            var result = new ResultDto<IEnumerable<TDto>>();
            try
            {
                var entities = await GetRepository().GetAllAsync();
                result.Data = _mapper.Map<IEnumerable<TDto>>(entities);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ActionFailed, "Unknown error");
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        // 🔹 GetAllQuery
        public async Task<IQueryable<TDto>> GetAllQuery()
        {
            var repo = GetRepository();
            IQueryable<dynamic> query = _dbType == DatabaseType.SqlServer
                ? ((ISqlRepository<TEntitySql>)repo).GetAllQueryable().AsNoTracking()
                : ((IPosgreSqlRepository<TEntityPosgreSql>)repo).GetAllQueryable().AsNoTracking();

            return query.ProjectTo<TDto>(_mapper.ConfigurationProvider);
        }
        public IQueryable<TEntitySql> Query()
        {
            return ((ISqlRepository<TEntitySql>)GetRepository())
                .GetAllQueryable()
                .AsNoTracking();
        }

        // 🔹 GetAllViewQueryable
        public async Task<IQueryable<TResult>> GetAllViewQueryable<PgView, SqlView, TResult>()
            where PgView : class
            where SqlView : class
            where TResult : class
        {
            try
            {
                IQueryable query = _dbType == DatabaseType.SqlServer
                    ? _sqlRepository.GetViewQueryable<SqlView>()
                    : _pgRepository.GetViewQueryable<PgView>();

                return query.ProjectTo<TResult>(_mapper.ConfigurationProvider).Cast<TResult>();
            }
            catch (Exception ex)
            {
                var errorMessage = await _messageService.Value.GetMessageAsync(LocalizationKeys.ActionFailed, "Unknown error");
                throw new Exception(errorMessage, ex);
            }
        }
        public async Task<IQueryable<TResult>> GetAllViewQueryable<SqlView, TResult>()
    where SqlView : class
    where TResult : class
        {
            try
            {
                IQueryable query = _sqlRepository.GetViewQueryable<SqlView>();


                return query.ProjectTo<TResult>(_mapper.ConfigurationProvider).Cast<TResult>();
            }
            catch (Exception ex)
            {
                var errorMessage = await _messageService.Value.GetMessageAsync(LocalizationKeys.ActionFailed, "Unknown error");
                throw new Exception(errorMessage, ex);
            }
        }

        public async Task<List<TResult>> GetAllViewListAsync<PgView, SqlView, TResult>()
    where PgView : class
    where SqlView : class
    where TResult : class
        {
            IQueryable query = _dbType == DatabaseType.SqlServer
                ? _sqlRepository.GetViewQueryable<SqlView>()
                : _pgRepository.GetViewQueryable<PgView>();

            // materialize
            return await query.ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();
        }

        // 🔹 GetByIdAsync
        public async Task<ResultDto<TDto?>> GetByIdAsync(params object[] idies)
        {
            var result = new ResultDto<TDto?>();
            try
            {
                var entity = await GetRepository().GetByIdAsync(idies);
                if (entity == null)
                {
                    result.Success = false;
                    result.Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.NotFound, "Not found");
                }
                else
                {
                    result.Data = _mapper.Map<TDto>(entity);
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ActionFailed, "Unknown error");
                result.Errors.Add(ex.Message);
            }
            return result;
        }


        // 🔹 CreateAsync
        public virtual async Task<ResultDto<TDto>> CreateAsync(TDto dto)
        {
            var result = new ResultDto<TDto>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();

            try
            {
                dynamic entity = _dbType == DatabaseType.SqlServer
                    ? _mapper.Map<TEntitySql>(dto)
                    : _mapper.Map<TEntityPosgreSql>(dto);
                var idProp = entity.GetType().GetProperty("Id");

                if (idProp != null && idProp.PropertyType == typeof(Guid))
                {
                    var currentId = (Guid)idProp.GetValue(entity);

                    if (currentId == Guid.Empty)
                    {
                        idProp.SetValue(entity, Guid.NewGuid());
                    }
                }
                var userId = _currentUserService.GetUserId();
                var CreatedAtProp = entity.GetType().GetProperty("CreatedAt");
                if (CreatedAtProp != null)
                {
                    // بررسی اینکه آیا نوع property دقیقا DateTime یا Nullable<DateTime> است
                    var type = Nullable.GetUnderlyingType(CreatedAtProp.PropertyType) ?? CreatedAtProp.PropertyType;
                    if (type == typeof(DateTime))
                    {
                        CreatedAtProp.SetValue(entity, DateTime.Now);
                    }
                }

                var CreatedByProp = entity.GetType().GetProperty("CreatedBy");
                if (CreatedByProp != null)
                {
                    var type = Nullable.GetUnderlyingType(CreatedByProp.PropertyType) ?? CreatedByProp.PropertyType;
                    if (type == typeof(Guid))
                    {
                        CreatedByProp.SetValue(entity, userId);
                    }
                }
 

                if (_dbType == DatabaseType.SqlServer)
                    await ((ISqlRepository<TEntitySql>)GetRepository()).InsertAsync(entity);
                else
                    await ((IPosgreSqlRepository<TEntityPosgreSql>)GetRepository()).InsertAsync(entity);

                result.Data = _mapper.Map<TDto>(entity);
                result.Success = true;
                result.Message = successMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = errorMessage;
                result.Errors.Add(ex.Message);
            }

            return result;
        }

        // 🔹 UpdateAsync
        public async Task<ResultDto<TDto?>> UpdateAsync<TDtoParam>(
            TDtoParam updatedDto,
            params object[] idies)
            where TDtoParam : class
        {
            var result = new ResultDto<TDto?>();
            var (successMessage, errorMessage) =
                await _messageService.Value.GetUpdateMessagesAsync();

            try
            {
                var repo = GetRepository();

                var existing = await repo.GetByIdAsync(idies);

                if (existing == null)
                {
                    result.Success = false;
                    result.Message = await _messageService.Value
                        .GetMessageAsync(LocalizationKeys.NotFound, "Not found");

                    return result;
                }

                var userId = _currentUserService.GetUserId();

                // -----------------------------
                // نگهداری مقادیر اصلی
                // -----------------------------
                var createdAtProp = existing.GetType().GetProperty("CreatedAt");
                var createdByProp = existing.GetType().GetProperty("CreatedBy");

                object? createdAtValue = createdAtProp?.GetValue(existing);
                object? createdByValue = createdByProp?.GetValue(existing);

                // -----------------------------
                // map dto -> entity
                // -----------------------------
                _mapper.Map(updatedDto, existing);

                // -----------------------------
                // برگرداندن CreatedAt
                // -----------------------------
                if (createdAtProp != null)
                {
                    var type = Nullable.GetUnderlyingType(createdAtProp.PropertyType)
                               ?? createdAtProp.PropertyType;

                    if (type == typeof(DateTime))
                    {
                        createdAtProp.SetValue(existing, createdAtValue);
                    }
                }

                // -----------------------------
                // برگرداندن CreatedBy
                // -----------------------------
                if (createdByProp != null)
                {
                    var type = Nullable.GetUnderlyingType(createdByProp.PropertyType)
                               ?? createdByProp.PropertyType;

                    if (type == typeof(Guid))
                    {
                        createdByProp.SetValue(existing, createdByValue);
                    }
                }

                // -----------------------------
                // UpdatedAt
                // -----------------------------
                var updatedAtProp = existing.GetType().GetProperty("UpdatedAt");

                if (updatedAtProp != null)
                {
                    var type = Nullable.GetUnderlyingType(updatedAtProp.PropertyType)
                               ?? updatedAtProp.PropertyType;

                    if (type == typeof(DateTime))
                    {
                        updatedAtProp.SetValue(existing, DateTime.UtcNow);
                    }
                }

                // -----------------------------
                // UpdatedBy
                // -----------------------------
                var updatedByProp = existing.GetType().GetProperty("UpdatedBy");

                if (updatedByProp != null)
                {
                    var type = Nullable.GetUnderlyingType(updatedByProp.PropertyType)
                               ?? updatedByProp.PropertyType;

                    if (type == typeof(Guid))
                    {
                        updatedByProp.SetValue(existing, userId);
                    }
                }

                await repo.UpdateAsync(existing);

                result.Data = _mapper.Map<TDto>(existing);
                result.Success = true;
                result.Message = successMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = errorMessage;
                result.Errors.Add(ex.Message);
            }

            return result;
        }

        // 🔹 DeleteAsync
        public async Task<ResultDto<bool>> DeleteAsync(Guid id)
        {
            var result = new ResultDto<bool>();
            var (successMessage, errorMessage) = await _messageService.Value.GetDeleteMessagesAsync();

            try
            {
                var repo = GetRepository();
                var existing = await repo.GetByIdAsync(id);

                if (existing == null)
                {
                    result.Success = false;
                    result.Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.NotFound, "Not found");
                    return result;
                }

                await repo.DeleteAsync(id);
                await repo.CommitAsync();
                result.Data = true;
                result.Success = true;
                result.Message = successMessage;
            }
            catch (DbUpdateException dbEx) when (
                (_dbType == DatabaseType.SqlServer && dbEx.InnerException is SqlException sqlEx && sqlEx.Number == 547) ||
                (_dbType == DatabaseType.PostgreSql && dbEx.InnerException is PostgresException pgEx && pgEx.SqlState == "23503")
            )
            {
                result.Success = false;
                result.Message = await _messageService.Value.GetMessageAsync(
                    LocalizationKeys.CannotDeleteUsedRecord,
                    "Cannot delete: this record is used in other tables. You can disable it instead."
                );
                result.Errors.Add(dbEx.Message);
                return result;
            }

            catch (Exception ex)
            {
                result.Success = false;
                result.Message = errorMessage;
                result.Errors.Add(ex.Message);
            }

            return result;
        }

        public async Task<ResultDto<IEnumerable<TDto>>> GetByPredicateAsync<TPredicateEntity>(
    Expression<Func<TPredicateEntity, bool>> predicate)
    where TPredicateEntity : class
        {
            var result = new ResultDto<IEnumerable<TDto>>();

            try
            {
                IQueryable<dynamic> query;

                if (_dbType == DatabaseType.SqlServer &&
                    typeof(TPredicateEntity) == typeof(TEntitySql))
                {
                    query = _sqlRepository
                        .GetAllQueryable()
                        .Where((Expression<Func<TEntitySql, bool>>)(object)predicate);
                }
                else if (_dbType == DatabaseType.PostgreSql &&
                         typeof(TPredicateEntity) == typeof(TEntityPosgreSql))
                {
                    query = _pgRepository
                        .GetAllQueryable()
                        .Where((Expression<Func<TEntityPosgreSql, bool>>)(object)predicate);
                }
                else
                {
                    throw new InvalidOperationException(
                        "Invalid entity type for the current database.");
                }

                var entities = await query.ToListAsync();

                result.Data = _mapper.Map<IEnumerable<TDto>>(entities);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = await _messageService.Value.GetMessageAsync(
                    LocalizationKeys.ActionFailed,
                    "Unknown error");

                result.Errors.Add(ex.Message);
            }

            return result;
        }

        // 🔹 FirstOrDefaultAsync
        public async Task<ResultDto<TDto?>> FirstOrDefaultAsync<TPredicateEntity>(Expression<Func<TPredicateEntity, bool>> predicate)
            where TPredicateEntity : class
        {
            var result = new ResultDto<TDto?>();
            try
            {
                dynamic entity;

                if (_dbType == DatabaseType.SqlServer && typeof(TPredicateEntity) == typeof(TEntitySql))
                    entity = await _sqlRepository.FirstOrDefaultAsync((Expression<Func<TEntitySql, bool>>)(object)predicate);
                else if (_dbType == DatabaseType.PostgreSql && typeof(TPredicateEntity) == typeof(TEntityPosgreSql))
                    entity = await _pgRepository.FirstOrDefaultAsync((Expression<Func<TEntityPosgreSql, bool>>)(object)predicate);
                else
                    throw new InvalidOperationException("Invalid entity type for the current database.");

                if (entity == null)
                {
                    result.Success = false;
                    result.Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.NotFound, "Not found");
                }
                else
                {
                    result.Data = _mapper.Map<TDto>(entity);
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ActionFailed, "Unknown error");
                result.Errors.Add(ex.Message);
            }

            return result;
        }
    }
}
