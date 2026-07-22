using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class ProductTagMappingService : GenericService<SqlProductTagMapping, SqlProductTagMapping, ProductTagMappingDto>, IProductTagMappingService
    {
        private readonly ISqlRepository<SqlProductTagMapping> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly Lazy<ILocalizationMessageService> _messageService;


        public ProductTagMappingService(
              ISqlRepository<SqlProductTagMapping> sqlRepository,
              IPosgreSqlRepository<SqlProductTagMapping> pgRepository,
              IMapper mapper,
              IConfiguration configuration,
              Lazy<ILocalizationMessageService> messageService
            , ICurrentUserService currentUserService
              )
              : base(sqlRepository, pgRepository, mapper, configuration,messageService, currentUserService)
        {
            _mapper = mapper;
            _messageService=messageService;
        }
        public async Task<ProductTagMappingDto?> GetByProductTagMappingIdAsync(Guid productId, Guid tagId)
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = (ISqlRepository<SqlProductTagMapping>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.ProductId == productId && x.ProductTagId == tagId);
                return _mapper.Map<ProductTagMappingDto>(entity);
            }
            else
            {
                var repo = (IPosgreSqlRepository<SqlProductTagMapping>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.ProductId == productId && x.ProductTagId == tagId);
                return _mapper.Map<ProductTagMappingDto>(entity);
            }
        }
        public async Task<List<ProductTagMappingDto>> GetByProductTagMappingsAsync(Guid productId)
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = (ISqlRepository<SqlProductTagMapping>)GetRepository();
                var entities = await repo.GetAll(c => c.ProductId == productId);
                return _mapper.Map<List<ProductTagMappingDto>>(entities); // ✅ مپ به لیست
            }
            else
            {
                var repo = (IPosgreSqlRepository<SqlProductTagMapping>)GetRepository();
                var entities = await repo.GetAll(c => c.ProductId == productId);
                return _mapper.Map<List<ProductTagMappingDto>>(entities); // ✅ مپ به لیست
            }
        }

        public async Task RemoveAsync(Guid productId, Guid tagId)
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = (ISqlRepository<SqlProductTagMapping>)GetRepository();
                var entity = await repo
             .FirstOrDefaultAsync(x =>
                 x.ProductId == productId &&
                 x.ProductTagId == tagId);
                if (entity == null)
                    return;
                await repo.RemoveAsync(entity);
                await repo.CommitAsync();
            }
            else
            {
                var repo = (IPosgreSqlRepository<SqlProductTagMapping>)GetRepository();
                var entity = await repo
        .FirstOrDefaultAsync(x =>
            x.ProductId == productId &&
            x.ProductTagId == tagId);
                if (entity == null)
                    return;
                await repo.RemoveAsync(entity);
                await repo.CommitAsync();
            }


        }


    }
}
