using HotChocolate.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Velora.Application.Shared.Services;

namespace Velora.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplateController : ControllerBase
    {
        private readonly IExcelTemplateService _excelTemplateService;

        public TemplateController(IExcelTemplateService excelTemplateService)
        {
            _excelTemplateService = excelTemplateService;
        }

        /// <summary>
        /// دانلود قالب اکسل برای مدل مشخص
        /// </summary>
        [HttpGet("Download/{entityName}")]
        [AllowAnonymous]
        public async Task<IActionResult> Download(string entityName)
        {
            if (string.IsNullOrWhiteSpace(entityName))
                return BadRequest("Entity name is required.");

            // 2️⃣ تولید فایل اکسل
            var fileBytes = await _excelTemplateService.GenerateTemplateWithLookupsAsync(entityName,1000);

            var fileName = $"{entityName}_Template.xlsx";

            // 3️⃣ بازگرداندن فایل
            return File(fileBytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }


    }

}
