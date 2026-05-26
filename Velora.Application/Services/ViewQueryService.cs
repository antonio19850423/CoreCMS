using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{
    public class ViewQueryService : IViewQueryService
    {
        private readonly ISqlRepository<object> _sqlRepository;
        private readonly IPosgreSqlRepository<object> _pgRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        private readonly DatabaseType _dbType;

        public ViewQueryService(
            ISqlRepository<object> sqlRepository,
            IPosgreSqlRepository<object> pgRepository,
            IMapper mapper,
            IConfiguration configuration)
        {
            _sqlRepository = sqlRepository;
            _pgRepository = pgRepository;
            _mapper = mapper;
            _configuration = configuration;

            var dbTypeString = configuration.GetValue<string>("Database:Provider") ?? "PostgreSql";
            _dbType = dbTypeString.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)
                ? DatabaseType.SqlServer
                : DatabaseType.PostgreSql;
        }

        private IQueryable GetView<TView>()
            where TView : class
        {
            return _dbType == DatabaseType.SqlServer
                ? _sqlRepository.GetViewQueryable<TView>()
                : _pgRepository.GetViewQueryable<TView>();
        }

        public async Task<List<TResult>> GetListAsync<TView, TResult>()
            where TView : class
            where TResult : class
        {
            var query = GetView<TView>();

            return await query
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TResult?> FirstOrDefaultAsync<TView, TResult>(
            Expression<Func<TResult, bool>> predicate)
            where TView : class
            where TResult : class
        {
            var query = GetView<TView>();

            return await query
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<IQueryable<TResult>> Query<TView, TResult>()
            where TView : class
            where TResult : class
        {
            var query = GetView<TView>();

            return query.ProjectTo<TResult>(_mapper.ConfigurationProvider);
        }
    }
}
