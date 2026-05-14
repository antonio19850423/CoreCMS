
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
    public class SeedHistoryService : GenericService<SqlSeedHistory,PgSeedHistory,SeedHistoryDto>, ISeedHistoryService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        public SeedHistoryService(
          ISqlRepository<SqlSeedHistory> sqlRepository,
          IPosgreSqlRepository<PgSeedHistory> pgRepository,
          IMapper mapper,
          IConfiguration configuration,Lazy<ILocalizationMessageService> messageService, ITransactionService transactionService, ICurrentUserService currentUserService)
          : base(sqlRepository, pgRepository, mapper, configuration, messageService,currentUserService)
        {
            _mapper = mapper;
            _messageService = messageService;
            _transactionService = transactionService;
        }

        public async Task<SeedHistoryDto?> GetByNameAsync(string name)
        {
            var repo = GetRepository();
            if (_dbType == DatabaseType.SqlServer)
            {
                var entity = await repo.FirstOrDefaultAsync((Expression<Func<SqlSeedHistory, bool>>)(x => x.Name.ToUpper() == name.ToUpper()));
                return _mapper.Map<SeedHistoryDto>(entity);

            }
            else
            {
                var entity = await repo.FirstOrDefaultAsync((Expression<Func<PgSeedHistory, bool>>)(x => x.Name.ToUpper() == name.ToUpper()));
                return _mapper.Map<SeedHistoryDto>(entity);

            }

        }
    }

}
