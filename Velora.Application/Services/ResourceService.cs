using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class ResourceService : GenericService<SqlResource, PgResource, ResourceDto>, IResourceService
    {
        private readonly ISqlRepository<PgResource> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResourceTypeService _resourceTypeService;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly Lazy<IPermissionService> _permissionService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        public ResourceService(
              ISqlRepository<SqlResource> sqlRepository,
              IPosgreSqlRepository<PgResource> pgRepository,
              IMapper mapper,
              IConfiguration configuration,
              IResourceTypeService resourceTypeService, IGeneralSettingService generalSettingService, Lazy<ILocalizationMessageService> messageService, ITransactionService transactionService, IModelValidationService modelValidationService, Lazy<IExcelTemplateService> excelTemplateService, ICurrentUserService currentUserService, Lazy<IPermissionService> permissionService)
              : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
        {
            _mapper = mapper;
            _resourceTypeService = resourceTypeService;
            _messageService = messageService;
            _transactionService = transactionService;
            _modelValidationService = modelValidationService;
            _excelTemplateService = excelTemplateService;
            _permissionService = permissionService;
        }
        public async Task<IQueryable<ResourcesViewDto>> GetAllViews()
        {
            return await GetAllViewQueryable<PgResourcesView, SqlResourcesView, ResourcesViewDto>();
        }

        public async Task<List<ResourcesViewDto>> GetAllMenusAsync(string languageCode)
        {
            var resourceTypeCode = "MENU";
            var resourceType = _dbType == DatabaseType.SqlServer
                ? await _resourceTypeService.FirstOrDefaultAsync<SqlResourceType>(x => x.Code.ToLower() == resourceTypeCode.ToLower())
                : await _resourceTypeService.FirstOrDefaultAsync<PgResourcetype>(x => x.Code.ToLower() == resourceTypeCode.ToLower());

            if (resourceType.Data == null)
                return new List<ResourcesViewDto>();

            var allowedResourceCodes = await _permissionService.Value.GetAllowedMenuResourceIdsAsync();
            if (!allowedResourceCodes.Any())
                return new List<ResourcesViewDto>();

            // 1️⃣ گرفتن همه ریسورس‌ها
            var resourcesQuery = await GetAllViewQueryable<PgResourcesView, SqlResourcesView, ResourcesViewDto>();
            var allMenus = resourcesQuery
                .Where(r => r.ResourceTypeId == resourceType.Data.Id
                            && r.LanguageCode == languageCode
                            && r.IsActive==true)
                .OrderBy(r => r.Order)
                .ToList();

            // 2️⃣ ساخت lookup برای Parent / Child
            var lookup = allMenus.ToLookup(m => m.ParentId);

            // 3️⃣ تابع بازگشتی برای فیلتر دسترسی و ساخت درخت
            List<ResourcesViewDto> BuildTree(Guid? parentId)
            {
                return lookup[parentId]
                    .Where(m => allowedResourceCodes.Contains(m.ResourceCode))
                    .OrderBy(m => m.Order)
                    .Select(m =>
                    {
                        m.Children = BuildTree(m.ResourceId); // حالا ResourceId هم Guid? هست
                        return m;
                    })
                    .ToList();
            }



            // 4️⃣ شروع از ریشه‌ها
            var rootMenus = BuildTree(null);
            return rootMenus;
        }


        public async Task<ResultDto<ResourceDto>> CreateAsync(ResourceCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ResourceDto>
                    {
                        Success = false,
                        Message= await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };

                var resource = _mapper.Map<ResourceDto>(input);
                var result = await CreateAsync(resource);
                if (!result.Success)
                    return result;

                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ResourceDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ResourceDto>> UpdateAsync(ResourceCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<ResourceDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ResourceDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var resource = _mapper.Map<ResourceDto>(input);

                var userResult = await UpdateAsync(resource, input.Id.Value);
                if (!userResult.Success)
                    return userResult;
                await _transactionService.CommitAsync();
                return userResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ResourceDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
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
            List<ResourcesViewDto> data;

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
            var resource = _mapper.Map<List<ResourceCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.Resource, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }

        public async Task<ResourceDto?> GetByCodeAsync(string code)
        {
            var repo = GetRepository(); // متد protected در GenericService
            if (_dbType == DatabaseType.SqlServer)
            {
                var entity = await repo.FirstOrDefaultAsync((Expression<Func<SqlResource, bool>>)(x => x.Code.ToUpper() == code.ToUpper()));
                return _mapper.Map<ResourceDto>(entity);
            }
            else
            {
                var entity = await repo.FirstOrDefaultAsync((Expression<Func<PgResource, bool>>)(x => x.Code.ToUpper() == code.ToUpper()));
                return _mapper.Map<ResourceDto>(entity);
            }

        }
    }
}
