using ApexWebAPI.DTOs.CountryDTOs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.ValidationRule
{
    public class CreateCountryValidation : AbstractValidator<CreateCountryDto>
    {
        public CreateCountryValidation(IStringLocalizer<CreateCountryValidation> localizer)
        {
            RuleFor(x => x.NameAz)
                .NotEmpty().WithMessage(_ => localizer["NameRequired"])
                .MaximumLength(100).WithMessage(_ => localizer["NameMaxLength"]);
        }
    }

    public class UpdateCountryValidation : AbstractValidator<UpdateCountryDto>
    {
        public UpdateCountryValidation(IStringLocalizer<CreateCountryValidation> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(_ => localizer["IdInvalid"]);

            RuleFor(x => x.NameAz)
                .NotEmpty().WithMessage(_ => localizer["NameRequired"])
                .MaximumLength(100).WithMessage(_ => localizer["NameMaxLength"]);
        }
    }
}
