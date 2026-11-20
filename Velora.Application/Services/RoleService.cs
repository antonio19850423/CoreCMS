using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq.Expressions;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Infrastructure;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class RoleService : GenericService<SqlRole, PgRole, RoleDto>, IRoleService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IModelValidationService _modelValidationService;
        public RoleService(ISqlRepository<SqlRole> sqlRepository, IPosgreSqlRepository<PgRole> pgRepository, IMapper mapper, IConfiguration configuration, Lazy<ILocalizationMessageService> messageService, ITransactionService transactionService, ICurrentUserService currentUserService, IModelValidationService modelValidationService) : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
        {
            _mapper = mapper;
            _messageService = messageService;
            _transactionService = transactionService;
            _modelValidationService = modelValidationService;
        }

        // متد اختصاصی Role
        public async Task<RoleDto?> GetByRoleCodeAsync(string code)
        {
            var repo = GetRepository(); // متد protected در GenericService
            var entity = await repo.FirstOrDefaultAsync((Expression<Func<PgRole, bool>>)(x => x.Code == code));
            return _mapper.Map<RoleDto>(entity);
        }

        public async Task<ResultDto<RoleDto>> CreateAsync(RoleCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {

                // 1️⃣ ایجاد کاربر
                var role = new RoleDto
                {
                    Code = input.Code,
                    Description = input.Description,
                    Name = input.Name,
                };
                var userResult = await CreateAsync(role);
                if (!userResult.Success)
                    return userResult;
                await _transactionService.CommitAsync();
                return userResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<RoleDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<RoleCrud>> CreateAdvancedAsync(RoleCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                // Validation
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                {
                    return new ResultDto<RoleCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                }
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
                        await GetAllQuery(),
                        input.Id,
                         (
                          x => x.Code.ToLower() == input.Code.Trim().ToLower(),
                          "این کد قبلاً ثبت شده است"
                         )
                    );
                if (errors.Any())
                {
                    return new ResultDto<RoleCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم ها تکراری است",
                        Errors = errors
                    };
                }
                var role = new RoleDto
                {
                    Code = input.Code,
                    Description = input.Description,
                    Name = input.Name
                };
                var result = await CreateWithViewAsync(role, GetRoleById, x => x.Id);
                if (result.Success)
                    result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<RoleCrud>
                {
                    Success = false,
                    Message = errorMessage
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<RoleCrud>> UpdateAsync(RoleCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                // Validation
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                {
                    return new ResultDto<RoleCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                }
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
                 await GetAllQuery(),
                 input.Id,
                 (x => x.Code == input.Code, "این کد قبلاً ثبت شده است")
             );
                if (errors.Any())
                {
                    return new ResultDto<RoleCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم ها تکراری است",
                        Errors = errors
                    };
                }
                if (input.Id == null)
                {
                    return new ResultDto<RoleCrud>
                    {
                        Success = false,
                        Message = "Id is required"
                    };
                }

                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new RoleDto
                {
                    Id = input.Id.Value,
                    Code = input.Code,
                    Name = input.Name,
                    Description = input.Description
                };
                var userResult = await UpdateAsync(userUpdateDto, input.Id.Value);
                if (!userResult.Success)
                {
                    return new ResultDto<RoleCrud>
                    {
                        Success = false,
                        Message = userResult.Message,
                        Errors = userResult.Errors
                    };
                }

                await _transactionService.CommitAsync();
                var viewResult = await GetRoleById(input.Id.Value);
                if (viewResult == null)
                {
                    return new ResultDto<RoleCrud>
                    {
                        Success = false,
                        Message = "اطلاعات پس از ویرایش قابل دریافت نیست."
                    };
                }

                return new ResultDto<RoleCrud>
                {
                    Success = true,
                    Message = successMessage,
                    Data = viewResult
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<RoleCrud>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<IEnumerable<RoleDto>> GetByNamesAsync(List<string> roles)
        {
            if (roles == null || !roles.Any())
                return Enumerable.Empty<RoleDto>();
            var upperRoles = roles.Select(r => r.ToUpper()).ToArray();
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = GetSqlRepository(); // ISqlRepository<PgRole>
                var list = repo.GetAll(x => upperRoles.Contains(x.Name.ToUpper())).Result.ToList();
                return _mapper.Map<IEnumerable<RoleDto>>(list);
            }
            else
            {
                var repo = GetPgRepository(); // IPosgreSqlRepository<PgRole>
                var list = repo.GetAll(x => upperRoles.Contains(x.Name.ToUpper())).Result.ToList();
                return _mapper.Map<IEnumerable<RoleDto>>(list);
            }
        }

        public async Task<RoleCrud?> GetRoleById(Guid id)
        {
            return await GetSqlViewByIdQueryable<SqlRole, RoleCrud>(id);
        }
    }
}