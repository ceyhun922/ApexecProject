using ApexWebAPI.DTOs.ContactInfoDTOs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.ValidationRule
{
    public class CreateContactValidation : AbstractValidator<CreateContactInfoDto>
    {
        public CreateContactValidation(IStringLocalizer<CreateContactValidation> localizer)
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(_ => localizer["PhoneRequired"])
                .MaximumLength(20).WithMessage(_ => localizer["PhoneMaxLength"]);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_ => localizer["EmailRequired"])
                .EmailAddress().WithMessage(_ => localizer["EmailInvalid"]);
        }
    }

    public class UpdateContactValidation : AbstractValidator<UpdateContactInfoDto>
    {
        public UpdateContactValidation(IStringLocalizer<CreateContactValidation> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(_ => localizer["IdInvalid"]);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(_ => localizer["PhoneRequired"])
                .MaximumLength(20).WithMessage(_ => localizer["PhoneMaxLength"]);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_ => localizer["EmailRequired"])
                .EmailAddress().WithMessage(_ => localizer["EmailInvalid"]);
        }
    }
}
