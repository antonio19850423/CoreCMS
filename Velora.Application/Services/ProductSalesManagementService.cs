using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class ProductSalesManagementService : GenericService<SqlProductSalesManagement, SqlProductSalesManagement, SqlProductSalesManagement>, IProductSalesManagementService
    {
        private readonly ISqlRepository<SqlProductSalesManagement> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IProductSalesManagementService _roleProductSalesManagementService;
        protected readonly ICurrentUserService _currentUserService;
        protected readonly IDiscountService _discountService;
        protected readonly IRoleService _roleService;
        protected readonly Lazy<IShoppingCartService> _shoppingCartService;
        public ProductSalesManagementService(
              ISqlRepository<SqlProductSalesManagement> sqlRepository,
              IPosgreSqlRepository<SqlProductSalesManagement> pgRepository,
              IMapper mapper,
              IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env,
              Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService,
              ICurrentUserService currentUserService, IDiscountService discountService, Lazy<IShoppingCartService> shoppingCartService, IRoleService roleService)
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
            _discountService = discountService;
            _shoppingCartService = shoppingCartService;
            _roleService = roleService;
        }
        public async Task<IQueryable<ProductSalesManagementCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlProductSalesManagement, SqlProductSalesManagement, ProductSalesManagementCrud>();
        }


        public async Task<byte[]> ExportAsync(
bool exportCurrentProductSalesManagement,
int ProductSalesManagementNumber,
int ProductSalesManagementSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<ProductSalesManagementCrud> data;

            if (exportCurrentProductSalesManagement)
            {
                data = query
                    .Skip((ProductSalesManagementNumber - 1) * ProductSalesManagementSize)
                    .Take(ProductSalesManagementSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<ProductSalesManagementCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.ProductSalesManagement, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }


    }

}
