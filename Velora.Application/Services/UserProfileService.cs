using AutoMapper;
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
    public class UserProfileService : GenericService<SqlUserProfile, PgUserProfile, UserProfileDto>, IUserProfileService
    {
        private readonly ISqlRepository<PgUserProfile> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        public UserProfileService(
              ISqlRepository<SqlUserProfile> sqlRepository,
              IPosgreSqlRepository<PgUserProfile> pgRepository,
              IMapper mapper,
              IConfiguration configuration,Lazy<ILocalizationMessageService> messageService, ICurrentUserService currentUserService)
              : base(sqlRepository, pgRepository, mapper,configuration,messageService,currentUserService)
        {
            _mapper = mapper;
            _messageService=messageService;
        }
        public async Task<UserProfileDto?> GetByUserIdAsync(Guid UserId)
            {
            if(_dbType == DatabaseType.SqlServer)
                {
                var repo = (ISqlRepository<SqlUserProfile>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.UserId == UserId);
                return _mapper.Map<UserProfileDto>(entity);
                }
            else
                {
                var repo = (IPosgreSqlRepository<PgUserProfile>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.UserId == UserId);
                return _mapper.Map<UserProfileDto>(entity);
                }
            }
        }

}
