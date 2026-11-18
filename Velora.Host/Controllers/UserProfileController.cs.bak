using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Shared;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
/// <summary>
/// /// <summary>
/// کنترلرهایی که در فرم‌های داینامیک استفاده می‌شوند،
/// حتماً باید دو متد BulkInsert و Export را پیاده‌سازی کنند.
///
/// BulkInsert:
/// برای ثبت گروهی اطلاعات از طریق فایل Excel یا فایل ورودی استفاده می‌شود.
///
/// Export:
/// برای تولید و دانلود فایل Excel اطلاعات Grid استفاده می‌شود.
///
/// وجود این متدها برای موارد زیر الزامی است:
/// - ثبت صحیح Resource و Permission ها
/// - شناسایی صحیح سرویس‌ها در فرانت‌اند
/// - جلوگیری از خطای Service not found
/// - فعال شدن قابلیت Import و Export اکسل
///
/// بعد از اضافه کردن این متدها:
/// - جدول SeedHistory پاک شود
/// - پروژه مجدداً اجرا شود
/// - EntityName داخل ModelMapping ثبت شود
/// </summary>
namespace Velora.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase, IDynamicFormController<UserProfileDto>
    {
        private readonly IUserProfileService _userProfileService;
        private readonly ITransactionService _transactionService;

        public UserProfileController(IUserProfileService userProfileService, ITransactionService transactionService)
        {
            _userProfileService = userProfileService;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userProfileService.GetAllAsync();
            return Ok(result); // result: ResultDto<List<UserProfileDto>>
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userProfileService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserProfileDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultDto<UserProfileDto>
                {
                    Success = false,
                    Message = "Invalid model",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var result = await _userProfileService.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            await _transactionService.CommitAsync();
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserProfileDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultDto<UserProfileDto>
                {
                    Success = false,
                    Message = "Invalid model",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            if (id != dto.Id)
                return BadRequest(new ResultDto<UserProfileDto> { Success = false, Message = "Id mismatch" });

            var result = await _userProfileService.UpdateAsync(dto,id);
            if (!result.Success)
                return NotFound(result);

            await _transactionService.CommitAsync();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userProfileService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result);

            await _transactionService.CommitAsync();
            return Ok(result);
        }

        Task<IActionResult> IDynamicFormController<UserProfileDto>.Update(UserProfileDto dto)
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> IDynamicFormController<UserProfileDto>.BulkInsert()
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> IDynamicFormController<UserProfileDto>.Export(ExportRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
