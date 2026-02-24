using ApexWebAPI.DTOs.InformationDTOs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.ValidationRule
{
    public class CreateInformationValidation : AbstractValidator<CreateInformationDto>
    {
        public CreateInformationValidation(IStringLocalizer<CreateInformationValidation> localizer)
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(_ => localizer["FullNameRequired"])
                .MaximumLength(100).WithMessage(_ => localizer["FullNameMaxLength"]);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(_ => localizer["PhoneRequired"])
                .MaximumLength(20).WithMessage(_ => localizer["PhoneMaxLength"]);

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage(_ => localizer["EmailInvalid"]);

            RuleFor(x => x.Education)
                .NotEmpty().WithMessage(_ => localizer["EducationRequired"])
                .MaximumLength(200).WithMessage(_ => localizer["EducationMaxLength"]);

            RuleFor(x => x.ClassOrYear)
                .NotEmpty().WithMessage(_ => localizer["ClassOrYearRequired"])
                .MaximumLength(20).WithMessage(_ => localizer["ClassOrYearMaxLength"]);
        }
    }

    public class UpdateInformationValidation : AbstractValidator<UpdateInformationDto>
    {
        public UpdateInformationValidation(IStringLocalizer<CreateInformationValidation> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(_ => localizer["IdInvalid"]);

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(_ => localizer["FullNameRequired"])
                .MaximumLength(100).WithMessage(_ => localizer["FullNameMaxLength"]);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(_ => localizer["PhoneRequired"])
                .MaximumLength(20).WithMessage(_ => localizer["PhoneMaxLength"]);

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage(_ => localizer["EmailInvalid"]);

            RuleFor(x => x.Education)
                .NotEmpty().WithMessage(_ => localizer["EducationRequired"])
                .MaximumLength(200).WithMessage(_ => localizer["EducationMaxLength"]);

            RuleFor(x => x.ClassOrYear)
                .NotEmpty().WithMessage(_ => localizer["ClassOrYearRequired"])
                .MaximumLength(20).WithMessage(_ => localizer["ClassOrYearMaxLength"]);
        }
    }
}
