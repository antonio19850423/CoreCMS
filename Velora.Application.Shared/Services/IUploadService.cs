using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
    {
    public interface IUploadService:IBaseService {

        Task<ResultDto<UploadResultDto?>> UploadImageAsync(IFormFile file,string name);
        }
    }
