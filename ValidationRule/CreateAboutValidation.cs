using ApexWebAPI.DTOs.AboutDTOs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.ValidationRule
{
    public class CreateAboutValidation : AbstractValidator<CreateAboutDto>
    {
        public CreateAboutValidation(IStringLocalizer<CreateAboutValidation> localizer)
        {
            RuleFor(x => x.TitleAz)
                .NotEmpty().WithMessage(_ => localizer["TitleRequired"])
                .MaximumLength(300).WithMessage(_ => localizer["TitleMaxLength"]);

            RuleFor(x => x.SubTitleAz)
                .NotEmpty().WithMessage(_ => localizer["SubTitleRequired"])
                .MaximumLength(500).WithMessage(_ => localizer["SubTitleMaxLength"]);
        }
    }

    public class UpdateAboutValidation : AbstractValidator<UpdateAboutDto>
    {
        public UpdateAboutValidation(IStringLocalizer<CreateAboutValidation> localizer)
        {
            RuleFor(x => x.TitleAz)
                .NotEmpty().WithMessage(_ => localizer["TitleRequired"])
                .MaximumLength(300).WithMessage(_ => localizer["TitleMaxLength"]);

            RuleFor(x => x.SubTitleAz)
                .NotEmpty().WithMessage(_ => localizer["SubTitleRequired"])
                .MaximumLength(500).WithMessage(_ => localizer["SubTitleMaxLength"]);
        }
    }
}
