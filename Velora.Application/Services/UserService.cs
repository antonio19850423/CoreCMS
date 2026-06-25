
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Extensions;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;
namespace Velora.Application.Services
{ 
    public class UserService : GenericService<SqlUser, PgUser, UserDto>, IUserService
    {
        private readonly ISqlRepository<PgUser> _repository;
        private readonly IUserProfileService _userProfileService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        public UserService(
              ISqlRepository<SqlUser> sqlRepository,
              IPosgreSqlRepository<PgUser> pgRepository,
              IMapper mapper,
              IConfiguration configuration, IUserProfileService userProfileService, IUserRoleService userRoleService, ITransactionService transactionService, IWebHostEnvironment env,
              Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService,
              ICurrentUserService currentUserService)
              : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
        {
            _mapper = mapper;
            _userProfileService = userProfileService;
            _userRoleService = userRoleService;
            _transactionService = transactionService;
            _messageService = messageService;
            _modelValidationService = modelValidationService;
            _env = env;
            _config = config;
            _excelTemplateService = excelTemplateService;
        }
        public async Task<UserDto?> GetByUserNameAsync(string UserName)
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = (ISqlRepository<SqlUser>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.UserName == UserName);
                return _mapper.Map<UserDto>(entity);
            }
            else
            {
                var repo = (IPosgreSqlRepository<PgUser>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.UserName == UserName);
                return _mapper.Map<UserDto>(entity);
            }
        }
        public async Task<IQueryable<UserCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<PgUserDetailView, SqlUserDetailView, UserCrud>();
        }
        public async Task<ResultDto<UserDto>> CreateAsync(UserCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<UserDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };



                // 1️⃣ ایجاد کاربر
                var user = new UserDto
                {
                    Email = input.Email,
                    IsActive = input.IsActive ?? true,
                    Password = BCrypt.Net.BCrypt.HashPassword(input.Password),
                    PhoneNumber = input.PhoneNumber,
                    UserName = input.UserName,
                    MobileNumber = input.MobileNumber,
                    NationalCode = input.NationalCode,
                };

                var userResult = await CreateAsync(user);
                if (!userResult.Success)
                    return userResult;

                var userId = userResult.Data.Id;

                // 2️⃣ ایجاد پروفایل
                var userProfile = new UserProfileDto
                {
                    Address = input.Address,
                    Firstname = input.FirstName,
                    Lastname = input.LastName,
                    Userid = userId,
                    ProfileImage = input.ProfileImage,
                    Age = input.Age,
                };
                await _userProfileService.CreateAsync(userProfile);

                var RoleIds = input.RoleIds?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(Guid.Parse)
                       .ToList();
                // دریافت نقش‌های فعلی کاربر
                var userRoles = await _userRoleService.GetByUserRolesAsync(userId);

                // حذف نقش‌هایی که در RoleIds جدید نیستند
                var rolesToRemove = userRoles
                    .Where(r => !RoleIds.Contains(r.Roleid))
                    .ToList();

                foreach (var r in rolesToRemove)
                {
                    await _userRoleService.DeleteAsync(r.Id); // فرض بر این است که UserRoleDto شامل Id است
                }
                // 3️⃣ نقش کاربر
                if (RoleIds.Any())
                {
                    foreach (var Role in RoleIds)
                    {
                        var userRole = new UserRoleDto
                        {
                            Roleid = Role,
                            Userid = userId
                        };
                        await _userRoleService.CreateAsync(userRole);
                    }

                }

                await _transactionService.CommitAsync();
                return userResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<UserDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<UserDto>> UpdateAsync(UserCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<UserDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<UserDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };

                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new UserDto
                {
                    Id = input.Id.Value,
                    Email = input.Email,
                    IsActive = input.IsActive ?? true,
                    PhoneNumber = input.PhoneNumber,
                    UserName = input.UserName
                };

                if (!string.IsNullOrWhiteSpace(input.Password))
                    userUpdateDto.Password = BCrypt.Net.BCrypt.HashPassword(input.Password);

                var userResult = await UpdateAsync(userUpdateDto, input.Id.Value);
                if (!userResult.Success)
                    return userResult;

                var userId = userResult.Data.Id;

                // 2️⃣ به‌روزرسانی یا ایجاد پروفایل
                var existingProfile = await _userProfileService
                    .GetByUserIdAsync(userId);

                if (existingProfile != null)
                {
                    var profileUpdate = existingProfile;
                    profileUpdate.Address = input.Address;
                    profileUpdate.Firstname = input.FirstName;
                    profileUpdate.Lastname = input.LastName;
                    profileUpdate.Nationalcode = input.NationalCode;
                    profileUpdate.Age = input.Age;
                    profileUpdate.ProfileImage = input.ProfileImage;
                    await _userProfileService.UpdateAsync(profileUpdate, profileUpdate.Id);
                }
                else
                {
                    var newProfile = new UserProfileDto
                    {
                        Address = input.Address,
                        Firstname = input.FirstName,
                        Lastname = input.LastName,
                        Nationalcode = input.NationalCode,
                        Userid = userId,
                        Age = input.Age,
                        ProfileImage = input.ProfileImage,
                    };
                    await _userProfileService.CreateAsync(newProfile);
                }
                var RoleIds = input.RoleIds?.Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(Guid.Parse)
        .ToList();
                // دریافت نقش‌های فعلی کاربر
                var userRoles = await _userRoleService.GetByUserRolesAsync(userId);

                // حذف نقش‌هایی که در RoleIds جدید نیستند
                var rolesToRemove = userRoles
                    .Where(r => !RoleIds.Contains(r.Roleid))
                    .ToList();

                foreach (var r in rolesToRemove)
                {
                    await _userRoleService.DeleteAsync(r.Id); // فرض بر این است که UserRoleDto شامل Id است
                }
                // 3️⃣ نقش کاربر
                if (RoleIds.Any())
                {
                    foreach (var Role in RoleIds)
                    {
                        var existingRole = await _userRoleService.GetByUserRoleIdAsync(userId,Role);

                        if (existingRole == null)
                        {
                            var newRole = new UserRoleDto
                            {
                                Roleid = Role,
                                Userid = userId
                            };
                            await _userRoleService.CreateAsync(newRole);
                        }
                    }

                }

                await _transactionService.CommitAsync();
                return userResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<UserDto>
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
            var createdUsers = new List<UserDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var users = dt.ToModelList<UserCrud>();

                for (int i = 0; i < users.Count; i++)
                {
                    var user = users[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(user);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdUsers.Add(createResult.Data);
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
                        InsertedCount = createdUsers.Count,
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
            List<UserCrud> data;

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
            var resource = _mapper.Map<List<UserCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.User, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }



    }
}
