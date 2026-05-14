using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Validators
{
    public class RoleDtoValidator : AbstractValidator<RoleDto>
    {

        public RoleDtoValidator(IStringLocalizerFactory factory)
        {
            var localizer = factory.Create("RoleDtoValidator", "Velora.Application");
            RuleFor(x => x.Name)
                .NotEmpty()
                 .WithMessage(x => string.Format(localizer["RequiredError"], "Name"))
                .MaximumLength(100)
                 .WithMessage(x => string.Format(localizer["MaxLengthError"], "Name", 100));
            RuleFor(x => x.Code)
                .NotEmpty()
                 .WithMessage(x => string.Format(localizer["RequiredError"], "Code"))
                .MaximumLength(100)
                 .WithMessage(x => string.Format(localizer["MaxLengthError"], "Code", 100));
        }
    }
}
