using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Extensions;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class PermissionService : GenericService<SqlPermission, PgPermission, PermissionDto>, IPermissionService
    {
        private readonly ISqlRepository<PgPermission> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IRolePermissionService _rolePermissionService;
        protected readonly ICurrentUserService _currentUserService;
        public PermissionService(
              ISqlRepository<SqlPermission> sqlRepository,
              IPosgreSqlRepository<PgPermission> pgRepository,
              IMapper mapper,
              IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env,
              Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService,
              ICurrentUserService currentUserService, IRolePermissionService rolePermissionService)
              : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
        {
            _mapper = mapper;
            _transactionService = transactionService;
            _messageService = messageService;
            _modelValidationService = modelValidationService;
            _env = env;
            _config = config;
            _excelTemplateService = excelTemplateService;
            _rolePermissionService = rolePermissionService;
            _currentUserService= currentUserService;
        }
        public async Task<IQueryable<PermissionCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<PgPermissionView, SqlPermissionView, PermissionCrud>();
        }
        public async Task<ResultDto<PermissionDto>> CreateAsync(PermissionCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<PermissionDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };



                var Permission = new PermissionDto
                {
                    Actions = input.Actions.Value,
                    ResourceId= input.ResourceId.Value,
                    IsActive = input.IsActive ?? true,
                };

                var PermissionResult = await CreateAsync(Permission);
                if (!PermissionResult.Success)
                    return PermissionResult;

                var PermissionId = PermissionResult.Data.Id;


                var RoleIds = input.RoleIds?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(Guid.Parse)
                       .ToList();
                // دریافت نقش‌های فعلی کاربر
                var PermissionRoles = await _rolePermissionService.GetByPermissionRolesAsync(PermissionId);

                // حذف نقش‌هایی که در RoleIds جدید نیستند
                var rolesToRemove = PermissionRoles
                    .Where(r => !RoleIds.Contains(r.RoleId))
                    .ToList();

                foreach (var r in rolesToRemove)
                {
                    await _rolePermissionService.DeleteAsync(r.Id); // فرض بر این است که PermissionRoleDto شامل Id است
                }
                // 3️⃣ نقش کاربر
                if (RoleIds.Any())
                {
                    foreach (var Role in RoleIds)
                    {
                        var PermissionRole = new RolePermissionDto
                        {
                            RoleId = Role,
                            PermissionId = PermissionId
                        };
                        await _rolePermissionService.CreateAsync(PermissionRole);
                    }

                }

                await _transactionService.CommitAsync();
                return PermissionResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<PermissionDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<PermissionDto>> UpdateAsync(PermissionCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<PermissionDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<PermissionDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };

                // 1️⃣ به‌روزرسانی کاربر
                var PermissionUpdateDto = new PermissionDto
                {
                    Id = input.Id.Value,
                    Actions=input.Actions.Value,
                    ResourceId=input.ResourceId.Value,
                    IsActive = input.IsActive ?? true,
                };

                var PermissionResult = await UpdateAsync(PermissionUpdateDto, input.Id.Value);
                if (!PermissionResult.Success)
                    return PermissionResult;

                var PermissionId = PermissionResult.Data.Id;

                var RoleIds = input.RoleIds?.Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(Guid.Parse)
        .ToList();
                // دریافت نقش‌های فعلی کاربر
                var PermissionRoles = await _rolePermissionService.GetByPermissionRolesAsync(PermissionId);

                // حذف نقش‌هایی که در RoleIds جدید نیستند
                var rolesToRemove = PermissionRoles
                    .Where(r => !RoleIds.Contains(r.RoleId))
                    .ToList();

                foreach (var r in rolesToRemove)
                {
                    await _rolePermissionService.DeleteAsync(r.Id); // فرض بر این است که PermissionRoleDto شامل Id است
                }
                // 3️⃣ نقش کاربر
                if (RoleIds.Any())
                {
                    foreach (var Role in RoleIds)
                    {
                        var existingRole = await _rolePermissionService.GetByPermissionRoleIdAsync(PermissionId, Role);

                        if (existingRole == null)
                        {
                            var newRole = new RolePermissionDto
                            {
                                RoleId = Role,
                                PermissionId = PermissionId
                            };
                            await _rolePermissionService.CreateAsync(newRole);
                        }
                    }

                }

                await _transactionService.CommitAsync();
                return PermissionResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<PermissionDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }
        public async Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream)
        {
            var createdPermissions = new List<PermissionDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var Permissions = dt.ToModelList<PermissionCrud>();

                for (int i = 0; i < Permissions.Count; i++)
                {
                    var Permission = Permissions[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(Permission);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdPermissions.Add(createResult.Data);
                    }
                    else
                    {
                        string errorMsg =
                            createResult.Errors != null && createResult.Errors.Any()
                                ? string.Join("; ", createResult.Errors)
                                : !string.IsNullOrWhiteSpace(createResult.Message)
                                    ? createResult.Message
                                    : "Unknown error";

                        dt.SetRowError(context.DataTableRowIndex, errorMsg);
                        errors.Add($"Row {context.ExcelRowNumber}: {errorMsg}");
                    }
                }

                await _transactionService.CommitAsync();

                string? errorFileUrl = null;
                if (errors.Any())
                {
                    errorFileUrl = dt.SaveErrorExcel(_env.WebRootPath!, _config);
                }

                return new ResultDto<BulkInsertResult>
                {
                    Success = errors.Count == 0,
                    Message = errors.Count == 0
                        ? successMessage
                        : errorFileTitle,
                    Data = new BulkInsertResult
                    {
                        InsertedCount = createdPermissions.Count,
                        ErrorCount = errors.Count,
                        ErrorFileUrl = errorFileUrl
                    },
                    Errors = errors
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                return new ResultDto<BulkInsertResult>
                {
                    Success = false,
                    Message = errorFileTitle,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<PermissionCrud> data;

            if (exportCurrentPage)
            {
                data = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<PermissionCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.Permission, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }

        public async Task<PermissionDto?> GetByResourceIdAsync(Guid resourceId)
        {
            var repo = GetRepository();
            if (_dbType == DatabaseType.SqlServer)
            {
                var entity = await repo.FirstOrDefaultAsync((Expression<Func<SqlPermission, bool>>)(x => x.ResourceId == resourceId));
                return _mapper.Map<PermissionDto>(entity);
            }
            else
            {
                var entity = await repo.FirstOrDefaultAsync((Expression<Func<PgPermission, bool>>)(x => x.ResourceId == resourceId));
                return _mapper.Map<PermissionDto>(entity);
            }
  
        }
        public async Task<HashSet<string>> GetAllowedMenuResourceIdsAsync()
        {
            var roles= _currentUserService.GetRoles();
            if (roles == null || !roles.Any())
                return new HashSet<string>();

            // 🔹 گرفتن map همه RolePermissionها
            var rolePermissionMap = await _rolePermissionService.GetRolePermissionMapAsync();

            // 🔹 فیلتر بر اساس Roleهای کاربر
            var allowedResourceCodes = rolePermissionMap
                .Where(rp => roles.Contains(rp.RoleId.ToString()))
                .SelectMany(rp => rp.ResourceCodes)
                .Distinct()
                .ToHashSet();

            // ⚠️ اینجا اگر ResourceId داری بهتره مستقیم Id برگردونی
            // ولی چون الان Code داری، در ResourceService مچ می‌کنیم

            return allowedResourceCodes;
        }






    }
}
