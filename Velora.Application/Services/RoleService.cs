
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq.Expressions;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;
namespace Velora.Application.Services
{
    public class RoleService : GenericService<SqlRole,PgRole,RoleDto>, IRoleService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        public RoleService(
          ISqlRepository<SqlRole> sqlRepository,
          IPosgreSqlRepository<PgRole> pgRepository,
          IMapper mapper,
          IConfiguration configuration,Lazy<ILocalizationMessageService> messageService, ITransactionService transactionService, ICurrentUserService currentUserService)
          : base(sqlRepository, pgRepository, mapper, configuration, messageService,currentUserService)
        {
            _mapper = mapper;
            _messageService = messageService;
            _transactionService = transactionService;
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

        public async Task<ResultDto<RoleDto>> UpdateAsync(RoleCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<RoleDto>
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



    }

}
