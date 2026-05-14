using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
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

        public ResourceTypeService(
              ISqlRepository<SqlResourceType> sqlRepository,
              IPosgreSqlRepository<PgResourcetype> pgRepository,
              IMapper mapper,
              IConfiguration configuration,
              Lazy<ILocalizationMessageService> messageService,
              ITransactionService transactionService
            , ICurrentUserService currentUserService
              )
              : base(sqlRepository, pgRepository, mapper, configuration,messageService, currentUserService)
        {
            _mapper = mapper;
            _messageService = messageService;
            _transactionService = transactionService;
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

        public async Task<ResultDto<ResourceTypeDto>> UpdateAsync(ResourceTypeCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<ResourceTypeDto>
                    {
                        Success = false,
                        Message = "Id is required"
                    };
                }

                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new ResourceTypeDto
                {
                    Id = input.Id,
                    Code = input.Code,
                    Name = input.Name,
                    Description = input.Description,
                    DisplayName= input.DisplayName,
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
                var result = new ResultDto<ResourceTypeDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }
    }

}
