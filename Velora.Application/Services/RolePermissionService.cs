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
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class RolePermissionService : GenericService<SqlRolePermission, PgRolePermission, RolePermissionDto>, IRolePermissionService
    {
        private readonly ISqlRepository<PgRolePermission> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly Lazy<ILocalizationMessageService> _messageService;


        public RolePermissionService(
              ISqlRepository<SqlRolePermission> sqlRepository,
              IPosgreSqlRepository<PgRolePermission> pgRepository,
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
        public async Task<RolePermissionDto?> GetByPermissionRoleIdAsync(Guid PermissionId, Guid RoleId)
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = (ISqlRepository<SqlRolePermission>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.PermissionId == PermissionId && x.RoleId == RoleId);
                return _mapper.Map<RolePermissionDto>(entity);
            }
            else
            {
                var repo = (IPosgreSqlRepository<PgRolePermission>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.PermissionId == PermissionId && x.RoleId == RoleId);
                return _mapper.Map<RolePermissionDto>(entity);
            }
        }
        public async Task<List<RolePermissionDto>> GetByPermissionRolesAsync(Guid PermissionId)
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = (ISqlRepository<SqlRolePermission>)GetRepository();
                var entities = await repo.GetAll(c => c.PermissionId == PermissionId);
                return _mapper.Map<List<RolePermissionDto>>(entities); // ✅ مپ به لیست
            }
            else
            {
                var repo = (IPosgreSqlRepository<PgRolePermission>)GetRepository();
                var entities = await repo.GetAll(c => c.PermissionId == PermissionId);
                return _mapper.Map<List<RolePermissionDto>>(entities); // ✅ مپ به لیست
            }
        }
        public async Task<List<RolePermissionMapDto>> GetRolePermissionMapAsync()
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = (ISqlRepository<SqlRolePermission>)GetRepository();

                // دریافت همه RolePermissionهای SQL همراه با Navigation Propertyها
                var queryable = await repo.GetAll(); // IQueryable<SqlRolePermission>
                var data = await queryable
                    .Include(rp => rp.Role)
                    .Include(rp => rp.Permission)
                        .ThenInclude(p => p.Resource)
                    .ToListAsync();

                return data
                    .GroupBy(x => new { x.Role.Id, x.Role.Name })
                    .Select(g => new RolePermissionMapDto
                    {
                        RoleId = g.Key.Id,
                        RoleName = g.Key.Name,
                        ResourceCodes = g.Select(x => x.Permission.Resource.Code).Distinct().ToList()
                    })
                    .ToList();
            }
            else
            {
                var repo = (IPosgreSqlRepository<PgRolePermission>)GetRepository();

                // دریافت همه RolePermissionهای PostgreSQL همراه با Navigation Propertyها
                var queryable = await repo.GetAll(); // IQueryable<PgRolePermission>
                var data = await queryable
                    .Include(rp => rp.Role)
                    .Include(rp => rp.Permission)
                        .ThenInclude(p => p.Resource)
                    .ToListAsync();

                return data
                    .GroupBy(x => new { x.Role.Id, x.Role.Name })
                    .Select(g => new RolePermissionMapDto
                    {
                        RoleId = g.Key.Id,
                        RoleName = g.Key.Name,
                        ResourceCodes = g.Select(x => x.Permission.Resource.Code).Distinct().ToList()
                    })
                    .ToList();
            }
        }

        public async Task RemoveAsync(Guid permissionId, Guid roleId)
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = (ISqlRepository<SqlRolePermission>)GetRepository();
                var entity = await repo
             .FirstOrDefaultAsync(x =>
                 x.PermissionId == permissionId &&
                 x.RoleId == roleId);
                if (entity == null)
                    return;
                await repo.RemoveAsync(entity);
                await repo.CommitAsync();
            }
            else
            {
                var repo = (IPosgreSqlRepository<PgRolePermission>)GetRepository();
                var entity = await repo
        .FirstOrDefaultAsync(x =>
            x.PermissionId == permissionId &&
            x.RoleId == roleId);
                if (entity == null)
                    return;
                await repo.RemoveAsync(entity);
                await repo.CommitAsync();
            }


        }


    }
}
