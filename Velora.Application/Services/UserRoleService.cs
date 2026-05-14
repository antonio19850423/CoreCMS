using AutoMapper;
using Microsoft.Extensions.Configuration;
using SharpCompress.Common;
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
    public class UserRoleService : GenericService<SqlUserRole, PgUserRole, UserRoleDto>, IUserRoleService
    {
        private readonly ISqlRepository<PgUserRole> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly Lazy<ILocalizationMessageService> _messageService;

        public UserRoleService(
              ISqlRepository<SqlUserRole> sqlRepository,
              IPosgreSqlRepository<PgUserRole> pgRepository,
              IMapper mapper,
              IConfiguration configuration,
              Lazy<ILocalizationMessageService> messageService
            , ICurrentUserService currentUserService
              )
              : base(sqlRepository, pgRepository, mapper, configuration,messageService, currentUserService)
        {
            _mapper = mapper;
            _messageService = messageService;
        }
        public async Task<List<RoleDto>> GetRolesByUserIdAsync(Guid userId)
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                return await GetSqlRepository().GetListAsync<RoleDto>(u => u.UserId == userId, c => new RoleDto { Id = c.RoleId, Name = c.Role.Name, Code = c.Role.Code }, u => u.Role);
            }
            else
            {
                return await GetPgRepository().GetListAsync<RoleDto>(u => u.UserId == userId, c => new RoleDto { Id=c.RoleId,Name = c.Role.Name, Code = c.Role.Code }, u => u.Role);
            }
        }
        public async Task<UserRoleDto?> GetByUserIdAsync(Guid UserId)
            {
            if(_dbType == DatabaseType.SqlServer)
                {
                var repo = (ISqlRepository<SqlUserRole>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.UserId == UserId);
                return _mapper.Map<UserRoleDto>(entity);
                }
            else
                {
                var repo = (IPosgreSqlRepository<PgUserRole>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.UserId == UserId);
                return _mapper.Map<UserRoleDto>(entity);
                }
            }
        public async Task<UserRoleDto?> GetByUserRoleIdAsync(Guid UserId,Guid RoleId)
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = (ISqlRepository<SqlUserRole>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.UserId == UserId && x.RoleId== RoleId);
                return _mapper.Map<UserRoleDto>(entity);
            }
            else
            {
                var repo = (IPosgreSqlRepository<PgUserRole>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.UserId == UserId && x.RoleId == RoleId);
                return _mapper.Map<UserRoleDto>(entity);
            }
        }
        public async Task<List<UserRoleDto>> GetByUserRolesAsync(Guid userId)
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = (ISqlRepository<SqlUserRole>)GetRepository();
                var entities = await repo.GetAll(c => c.UserId == userId);
                return _mapper.Map<List<UserRoleDto>>(entities); // ✅ مپ به لیست
            }
            else
            {
                var repo = (IPosgreSqlRepository<PgUserRole>)GetRepository();
                var entities = await repo.GetAll(c => c.UserId == userId);
                return _mapper.Map<List<UserRoleDto>>(entities); // ✅ مپ به لیست
            }
        }

        public async Task<IQueryable<UserRoleViewDto>> GetPgUserRolesView()
        {

            return await GetAllViewQueryable<PgUserRolesView, SqlUserRolesView, UserRoleViewDto>();
        }

        public async Task<IQueryable<UserRoleViewDto>> GetSqlUserRolesView()
        {
            return await GetAllViewQueryable<PgUserRolesView, SqlUserRolesView, UserRoleViewDto>();
        }
    }

}
