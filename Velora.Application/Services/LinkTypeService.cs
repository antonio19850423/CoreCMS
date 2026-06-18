using AutoMapper;
using GreenDonut;
using Microsoft.AspNetCore.Hosting;
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
    public class LinkTypeService : GenericService<SqlLinkType, SqlLinkType, LinkTypeDto>, ILinkTypeService
    {
        private readonly ISqlRepository<SqlLinkType> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly ILinkTypeService _roleLinkTypeService;
        protected readonly ICurrentUserService _currentUserService;
        protected readonly ICmsConfigurationService _cmsConfigurationService;
        public LinkTypeService(
              ISqlRepository<SqlLinkType> sqlRepository,
              IPosgreSqlRepository<SqlLinkType> pgRepository,
              IMapper mapper,
              IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env,
              Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService,
              ICurrentUserService currentUserService, ICmsConfigurationService cmsConfigurationService)
              : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
        {
            _mapper = mapper;
            _transactionService = transactionService;
            _messageService = messageService;
            _modelValidationService = modelValidationService;
            _env = env;
            _config = config;
            _excelTemplateService = excelTemplateService;
            _currentUserService = currentUserService;
            _cmsConfigurationService = cmsConfigurationService;
        }

        public async Task<IQueryable<LinkTypeCrud>> GetAllViews()
        {
            var configResult = await _cmsConfigurationService
                .FirstOrDefaultAsync<SqlCmsConfiguration>(x => x.IsActive);

            var query = await GetAllViewQueryable<SqlLinkTypeView, SqlLinkTypeView, LinkTypeCrud>();

            if (configResult.Data == null)
                return query;

            var config = configResult.Data;

            query = query.Where(c =>
                              (!config.EnableBlog || (c.Code != "ARTICLE"))
                              &&
                              (!config.EnableShop || c.Code != "PRODUCT")
                              &&
                              (!config.EnableNews || c.Code != "NEWS")
                              );
            return query;
        }
        public async Task<ResultDto<LinkTypeDto>> CreateAsync(LinkTypeCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {

                var LinkType = new LinkTypeDto
                {
                    Code = input.Code,
                    IsActive = input.IsActive,
                    Name = input.Name,
                    SortOrder = input.SortOrder,
                };

                var result = await CreateAsync(LinkType);
                if (!result.Success)
                    return result;

                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<LinkTypeDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<LinkTypeDto>> UpdateAsync(LinkTypeCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<LinkTypeDto>
                    {
                        Success = false,
                        Message = "Id is required"
                    };
                }

                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new LinkTypeDto
                {
                    Id = input.Id,
                    Code = input.Code,
                    Name = input.Name,
                    SortOrder = input.SortOrder,
                };

                var result = await UpdateAsync(userUpdateDto, input.Id);
                if (!result.Success)
                    return result;
                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<LinkTypeDto>
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
            var createdLinkTypes= new List<LinkTypeDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var LinkTypes = dt.ToModelList<LinkTypeCrud>();

                for (int i = 0; i < LinkTypes.Count; i++)
                {
                    var LinkType = LinkTypes[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(LinkType);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdLinkTypes.Add(createResult.Data);
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
                        InsertedCount = createdLinkTypes.Count,
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
            List<LinkTypeCrud> data;

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
            var resource = _mapper.Map<List<LinkTypeCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.LinkType, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }
        public async Task<ResultDto<List<LinkTypeDto>>> AddRangeAsync(List<LinkTypeDto> inputs)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();

            try
            {
                var resultList = new List<LinkTypeDto>();

                foreach (var input in inputs)
                {
                    var entity = new SqlLinkType
                    {
                        Id = Guid.NewGuid(),
                        Code = input.Code,
                        Name = input.Name,
                        SortOrder = input.SortOrder,
                        IsActive = input.IsActive,
                        IsTest = input.IsTest,
                        IsDeleted = false,
                        CreatedAt = DateTime.Now
                    };

                    await _sqlrepository.InsertAsync(entity);

                    resultList.Add(_mapper.Map<LinkTypeDto>(entity));
                }

                await _transactionService.CommitAsync();

                return new ResultDto<List<LinkTypeDto>>
                {
                    Success = true,
                    Data = resultList,
                    Message = successMessage
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();

                return new ResultDto<List<LinkTypeDto>>
                {
                    Success = false,
                    Message = errorMessage,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

    }

}
