
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
    public class LocalizationtranslationService : GenericService<SqlLocalizationtranslation, PgLocalizationtranslation, LocalizationtranslationDto>, ILocalizationtranslationService
    {
        private readonly IMapper _mapper;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        public LocalizationtranslationService(
          ISqlRepository<SqlLocalizationtranslation> sqlRepository,
          IPosgreSqlRepository<PgLocalizationtranslation> pgRepository,
          IMapper mapper,
          IConfiguration configuration,
          Lazy<ILocalizationMessageService> messageService, ICurrentUserService currentUserService
          )
          : base(sqlRepository, pgRepository, mapper, configuration,messageService, currentUserService)
        {
            _mapper = mapper;
            _messageService = messageService;
        }
        /// <summary>
        /// دریافت یک رکورد GeneralSetting بر اساس Key
        /// </summary>
        public async Task<LocalizationtranslationDto?> GetByCodeAsync(string code,string languageCode)
        {
            if (_dbType == DatabaseType.SqlServer)
            {
                var repo = (ISqlRepository<SqlLocalizationtranslation>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.LocalizationKeyCode.ToLower() == code.ToLower() && x.LanguageCode.ToLower() == languageCode.ToLower());
                return _mapper.Map<LocalizationtranslationDto>(entity);
            }
            else
            {
                var repo = (IPosgreSqlRepository<PgLocalizationtranslation>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.LocalizationKeyCode.ToLower() == code.ToLower() && x.LanguageCode.ToLower() == languageCode.ToLower());
                return _mapper.Map<LocalizationtranslationDto>(entity);
            }
        }
        public async Task<Dictionary<string,string>> GetTranslationsByLanguageAsync(string languageCode)
            {
            if(_dbType == DatabaseType.SqlServer)
                {
                var repo = (ISqlRepository<SqlLocalizationtranslation>)GetRepository();

                // گرفتن لیست رکوردها
                var entities = await repo.GetListAsync(
                    x => x.LanguageCode.ToLower() == languageCode.ToLower(),
                    c => new { c.LocalizationKeyCode,c.Value }
                );

                // تبدیل به دیکشنری
                return entities.ToDictionary(
                    x => x.LocalizationKeyCode,
                    x => x.Value
                );
                }
            else
                {
                var repo = (IPosgreSqlRepository<PgLocalizationtranslation>)GetRepository();

                // گرفتن لیست رکوردها (نه فقط یک رکورد)
                var entities = await repo.GetListAsync(
                    x => x.LanguageCode.ToLower() == languageCode.ToLower(),
                    c => new { c.LocalizationKeyCode,c.Value }
                );

                return entities.ToDictionary(
                    x => x.LocalizationKeyCode,
                    x => x.Value
                );
                }
            }


        }

    }
