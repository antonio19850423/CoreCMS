using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    public class ResourceLanguageService : GenericService<SqlResourceLanguage, PgResourceLanguage, ResourceLanguageDto>, IResourceLanguageService
    {
        private readonly ISqlRepository<PgResourceLanguage> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        public ResourceLanguageService(
              ISqlRepository<SqlResourceLanguage> sqlRepository,
              IPosgreSqlRepository<PgResourceLanguage> pgRepository,
              IMapper mapper,
              IConfiguration configuration,Lazy<ILocalizationMessageService> messageService, ICurrentUserService currentUserService)
              : base(sqlRepository, pgRepository, mapper, configuration,messageService, currentUserService)
        {
            _mapper = mapper;
            _messageService = messageService;
        }


    }
}
