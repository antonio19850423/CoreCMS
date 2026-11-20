using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Infrastructure;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class ResourceTypeService : GenericService<SqlResourceType, PgResourcetype, ResourceTypeDto>, IResourceTypeService
    {
        private readonly ISqlRepository<PgResourcetype> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IModelValidationService _modelValidationService;
        
        public ResourceTypeService(ISqlRepository<SqlResourceType> sqlRepository, IPosgreSqlRepository<PgResourcetype> pgRepository, IMapper mapper, IConfiguration configuration, Lazy<ILocalizationMessageService> messageService, ITransactionService transactionService, ICurrentUserService currentUserService, IModelValidationService modelValidationService) : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
        {
            _mapper = mapper;
            _messageService = messageService;
            _transactionService = transactionService;
            _modelValidationService = modelValidationService;
        }

        public async Task<ResultDto<ResourceTypeDto>> CreateAsync(ResourceTypeCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var resourceType = new ResourceTypeDto
                {
                    Code = input.Code,
                    DisplayName = input.DisplayName,
                    Description = input.Description,
                    Name = input.Name,
                };
                var result = await CreateAsync(resourceType);
                if (!result.Success)
                    return result;
                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ResourceTypeDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ResourceTypeCrud>> CreateAdvancedAsync(ResourceTypeCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                // Validation
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                {
                    return new ResultDto<ResourceTypeCrud>
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
                    return new ResultDto<ResourceTypeCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم ها تکراری است",
                        Errors = errors
                    };
                }
                var resourceType = new ResourceTypeDto
                {
                    Code = input.Code,
                    DisplayName = input.DisplayName,
                    Description = input.Description,
                    Name = input.Name
                };
                var result = await CreateWithViewAsync(resourceType, GetResourceTypeById, x => x.Id);
                if (result.Success)
                    result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ResourceTypeCrud>
                {
                    Success = false,
                    Message = errorMessage
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ResourceTypeCrud>> UpdateAsync(ResourceTypeCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<ResourceTypeCrud>
                    {
                        Success = false,
                        Message = "Id is required"
                    };
                }
                // Validation
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                {
                    return new ResultDto<ResourceTypeCrud>
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
                    return new ResultDto<ResourceTypeCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم ها تکراری است",
                        Errors = errors
                    };
                }
                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new ResourceTypeDto
                {
                    Id = input.Id,
                    Code = input.Code,
                    Name = input.Name,
                    Description = input.Description,
                    DisplayName = input.DisplayName,
                };
                var result = await UpdateAsync(userUpdateDto, input.Id);
                if (!result.Success)
                {
                    return new ResultDto<ResourceTypeCrud>
                    {
                        Success = false,
                        Message = result.Message,
                        Errors = result.Errors
                    };
                }

                await _transactionService.CommitAsync();
                var viewResult = await GetResourceTypeById(input.Id);
                if (viewResult == null)
                {
                    return new ResultDto<ResourceTypeCrud>
                    {
                        Success = false,
                        Message = "اطلاعات پس از ویرایش قابل دریافت نیست."
                    };
                }

                return new ResultDto<ResourceTypeCrud>
                {
                    Success = true,
                    Message = successMessage,
                    Data = viewResult
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ResourceTypeCrud>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResourceTypeCrud?> GetResourceTypeById(Guid id)
        {
            return await GetSqlViewByIdQueryable<SqlResourceType, ResourceTypeCrud>(id);
        }
    }
}