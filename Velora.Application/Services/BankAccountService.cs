using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
using Velora.Application.Shared.Infrastructure;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class BankAccountService : GenericService<SqlBankAccount, SqlBankAccount, BankAccountDto>, IBankAccountService
    {
        private readonly ISqlRepository<SqlBankAccount> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IBankAccountService _roleBankAccountService;
        protected readonly ICurrentUserService _currentUserService;
        public BankAccountService(ISqlRepository<SqlBankAccount> sqlRepository, IPosgreSqlRepository<SqlBankAccount> pgRepository, IMapper mapper, IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env, Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService, ICurrentUserService currentUserService) : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
        {
            _mapper = mapper;
            _transactionService = transactionService;
            _messageService = messageService;
            _modelValidationService = modelValidationService;
            _env = env;
            _config = config;
            _excelTemplateService = excelTemplateService;
            _currentUserService = currentUserService;
        }

        public async Task<IQueryable<BankAccountCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlBankAccountView, SqlBankAccountView, BankAccountCrud>();
        }

        public async Task<IQueryable<BankAccountCrud>> GetBankAccountsBySiteInfoId(Guid siteInfoId)
        {
            var result = await GetAllViewQueryable<SqlBankAccountView, SqlBankAccountView, BankAccountCrud>();
            return result.Where(c => c.ParentId == siteInfoId);
        }

        public async Task<ResultDto<BankAccountDto>> CreateAsync(BankAccountCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<BankAccountDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var BankAccount = new BankAccountDto
                {
                    IsActive = input.IsActive,
                    SiteSettingId = input.ParentId,
                    AccountNumber = input.AccountNumber,
                    AccountOwnerName = input.AccountOwnerName,
                    BankName = input.BankName,
                    CardNumber = input.CardNumber,
                    Description = input.Description,
                    DisplayOrder = input.DisplayOrder,
                    IsDefault = input.IsDefault,
                    ShebaNumber = input.ShebaNumber
                };
                var BankAccountResult = await CreateAsync(BankAccount);
                await _transactionService.CommitAsync();
                return BankAccountResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<BankAccountDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<BankAccountCrud>> CreateAdvancedAsync(BankAccountCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<BankAccountCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
                      await GetAllQuery(),
                      input.Id,
                      (
                          x => (x.CardNumber != null &&
                             input.CardNumber != null &&
                             x.CardNumber.ToLower() == input.CardNumber.ToLower()),
                              "این شماره کارت قبلاً ثبت شده است"
                      ),
                               (
                          x => (x.AccountNumber != null &&
                             input.AccountNumber != null &&
                             x.AccountNumber.ToLower() == input.AccountNumber.ToLower()),
                          "این شماره حساب قبلاً ثبت شده است"
                      )
                      ,
                               (
                          x => (x.ShebaNumber != null &&
                             input.ShebaNumber != null &&
                             x.ShebaNumber.ToLower() == input.ShebaNumber.ToLower()),
                          "این شماره شبا قبلاً ثبت شده است"
                      )
                  );
                if (errors.Any())
                {
                    return new ResultDto<BankAccountCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم‌ها تکراری است",
                        Errors = errors
                    };
                }
                var bankAccount = new BankAccountDto
                {
                    IsActive = input.IsActive,
                    SiteSettingId = input.ParentId,
                    AccountNumber = input.AccountNumber,
                    AccountOwnerName = input.AccountOwnerName,
                    BankName = input.BankName,
                    CardNumber = input.CardNumber,
                    Description = input.Description,
                    DisplayOrder = input.DisplayOrder,
                    IsDefault = input.IsDefault,
                    ShebaNumber = input.ShebaNumber
                };
                if (bankAccount.DisplayOrder <= 0)
                {
                    bankAccount.DisplayOrder = (await Query().MaxAsync(x => (int?)x.DisplayOrder) ?? 0) + 1;
                }
                var result = await CreateWithViewAsync(bankAccount, GetBankAccountById, x => x.Id);
                if (result.Success)
                    result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<BankAccountCrud>
                {
                    Success = false,
                    Message = errorMessage
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<BankAccountCrud>> UpdateAsync(BankAccountCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<BankAccountCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<BankAccountCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
                      await GetAllQuery(),
                      input.Id,
                      (
                          x => (x.CardNumber != null &&
                             input.CardNumber != null &&
                             x.CardNumber.ToLower() == input.CardNumber.ToLower()),
                              "این شماره کارت قبلاً ثبت شده است"
                      ),
                               (
                          x => (x.AccountNumber != null &&
                             input.AccountNumber != null &&
                             x.AccountNumber.ToLower() == input.AccountNumber.ToLower()),
                          "این شماره حساب قبلاً ثبت شده است"
                      )
                      ,
                               (
                          x => (x.ShebaNumber != null &&
                             input.ShebaNumber != null &&
                             x.ShebaNumber.ToLower() == input.ShebaNumber.ToLower()),
                          "این شماره شبا قبلاً ثبت شده است"
                      )
                  );
                if (errors.Any())
                {
                    return new ResultDto<BankAccountCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم‌ها تکراری است",
                        Errors = errors
                    };
                }
                // 1️⃣ به‌روزرسانی کاربر
                var updateDto = new BankAccountDto
                {
                    Id = input.Id,
                    IsActive = input.IsActive,
                    SiteSettingId = input.ParentId,
                    AccountNumber = input.AccountNumber,
                    AccountOwnerName = input.AccountOwnerName,
                    BankName = input.BankName,
                    CardNumber = input.CardNumber,
                    Description = input.Description,
                    DisplayOrder = input.DisplayOrder,
                    IsDefault = input.IsDefault,
                    ShebaNumber = input.ShebaNumber
                };
                var BankAccountResult = await UpdateAsync(updateDto, input.Id);
                if (!BankAccountResult.Success)
                {
                    return new ResultDto<BankAccountCrud>
                    {
                        Success = false,
                        Message = BankAccountResult.Message,
                        Errors = BankAccountResult.Errors
                    };
                }

                await _transactionService.CommitAsync();
                var viewResult = await GetBankAccountById(input.Id);
                if (viewResult == null)
                {
                    return new ResultDto<BankAccountCrud>
                    {
                        Success = false,
                        Message = "اطلاعات پس از ویرایش قابل دریافت نیست."
                    };
                }

                return new ResultDto<BankAccountCrud>
                {
                    Success = true,
                    Message = successMessage,
                    Data = viewResult
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<BankAccountCrud>
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
            var createdBankAccounts = new List<BankAccountDto>();
            var errors = new List<string>();
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var(dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var BankAccounts = dt.ToModelList<BankAccountCrud>();
                for (int i = 0; i < BankAccounts.Count; i++)
                {
                    var BankAccount = BankAccounts[i];
                    var context = rowContexts[i];
                    var createResult = await CreateAsync(BankAccount);
                    if (createResult.Success && createResult.Data != null)
                    {
                        createdBankAccounts.Add(createResult.Data);
                    }
                    else
                    {
                        string errorMsg = createResult.Errors != null && createResult.Errors.Any() ? string.Join("; ", createResult.Errors) : !string.IsNullOrWhiteSpace(createResult.Message) ? createResult.Message : "Unknown error";
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
                    Message = errors.Count == 0 ? successMessage : errorFileTitle,
                    Data = new BulkInsertResult
                    {
                        InsertedCount = createdBankAccounts.Count,
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
                    Errors = new List<string>
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<byte[]> ExportAsync(bool exportCurrentBankAccount, int BankAccountNumber, int BankAccountSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>
            // 2️⃣ Paging و Mapping به DTO
            List<BankAccountCrud> data;
            if (exportCurrentBankAccount)
            {
                data = query.Skip((BankAccountNumber - 1) * BankAccountSize).Take(BankAccountSize).ToList();
            }
            else
            {
                data = query.ToList();
            }

            var resource = _mapper.Map<List<BankAccountCrud>>(data);
            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(LookupEntities.BankAccount, // نام مدل DTO
 data.Count + 5);
            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);
            return resultBytes;
        }

        public async Task<BankAccountCrud?> GetBankAccountById(Guid id)
        {
            return await GetSqlViewByIdQueryable<SqlBankAccountView, BankAccountCrud>(id);
        }
    }
}