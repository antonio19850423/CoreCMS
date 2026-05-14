
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq.Expressions;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;
namespace Velora.Application.Services
{
    public class LocalizationkeyService : GenericService<SqlLocalizationKey,PgLocalizationkey,LocalizationkeyDto>, ILocalizationkeyService
    {
        private readonly IMapper _mapper;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        public LocalizationkeyService(
          ISqlRepository<SqlLocalizationKey> sqlRepository,
          IPosgreSqlRepository<PgLocalizationkey> pgRepository,
          IMapper mapper,
          IConfiguration configuration,
          Lazy<ILocalizationMessageService> messageService, ICurrentUserService currentUserService)
          : base(sqlRepository, pgRepository, mapper, configuration,messageService, currentUserService)
        {
            _mapper = mapper;
            _messageService= messageService;
        }

        // متد اختصاصی Role
        public async Task<RoleDto?> GetByCodeAsync(string code)
        {
            var repo = GetRepository(); // متد protected در GenericService
            var entity = await repo.FirstOrDefaultAsync((Expression<Func<PgLocalizationkey, bool>>)(x => x.Code == code));
            return _mapper.Map<LocalizationkeyDto>(entity);
        }
    }

}
