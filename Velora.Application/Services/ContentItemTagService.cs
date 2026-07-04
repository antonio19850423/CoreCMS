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
    public class ContentItemTagService : GenericService<SqlContentItemTag, SqlContentItemTag, ContentItemTagDto>, IContentItemTagService
    {
        private readonly ISqlRepository<SqlContentItemTag> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly Lazy<ILocalizationMessageService> _messageService;


        public ContentItemTagService(
              ISqlRepository<SqlContentItemTag> sqlRepository,
              IPosgreSqlRepository<SqlContentItemTag> pgRepository,
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
        public async Task<ContentItemTagDto?> GetByContentItemTagIdAsync(Guid contentItemId, Guid tagId)
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = (ISqlRepository<SqlContentItemTag>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.ContentItemId == contentItemId && x.TagId == tagId);
                return _mapper.Map<ContentItemTagDto>(entity);
            }
            else
            {
                var repo = (IPosgreSqlRepository<SqlContentItemTag>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.ContentItemId == contentItemId && x.TagId == tagId);
                return _mapper.Map<ContentItemTagDto>(entity);
            }
        }
        public async Task<List<ContentItemTagDto>> GetByContentItemTagsAsync(Guid ContentItemId)
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = (ISqlRepository<SqlContentItemTag>)GetRepository();
                var entities = await repo.GetAll(c => c.ContentItemId == ContentItemId);
                return _mapper.Map<List<ContentItemTagDto>>(entities); // ✅ مپ به لیست
            }
            else
            {
                var repo = (IPosgreSqlRepository<SqlContentItemTag>)GetRepository();
                var entities = await repo.GetAll(c => c.ContentItemId == ContentItemId);
                return _mapper.Map<List<ContentItemTagDto>>(entities); // ✅ مپ به لیست
            }
        }

        public async Task RemoveAsync(Guid contentItemId, Guid tagId)
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = (ISqlRepository<SqlContentItemTag>)GetRepository();
                var entity = await repo
             .FirstOrDefaultAsync(x =>
                 x.ContentItemId == contentItemId &&
                 x.TagId == tagId);
                if (entity == null)
                    return;
                await repo.RemoveAsync(entity);
                await repo.CommitAsync();
            }
            else
            {
                var repo = (IPosgreSqlRepository<SqlContentItemTag>)GetRepository();
                var entity = await repo
        .FirstOrDefaultAsync(x =>
            x.ContentItemId == contentItemId &&
            x.TagId == tagId);
                if (entity == null)
                    return;
                await repo.RemoveAsync(entity);
                await repo.CommitAsync();
            }


        }


    }
}
