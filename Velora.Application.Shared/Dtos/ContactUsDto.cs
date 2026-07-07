using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class ContactUsDto
    {
        [Required(ErrorMessage = "نام الزامی است.")]
        public string FirstName { get; set; } = default!;

        [Required(ErrorMessage = "نام خانوادگی الزامی است.")]
        public string LastName { get; set; } = default!;

        [Required(ErrorMessage = "ایمیل الزامی است.")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست.")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "پیام الزامی است.")]
        public string Message { get; set; } = default!;
    }
}
