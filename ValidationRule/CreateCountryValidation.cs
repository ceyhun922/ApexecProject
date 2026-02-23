using ApexWebAPI.DTOs.CountryDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class CreateCountryValidation : AbstractValidator<CreateCountryDto>
    {
        public CreateCountryValidation()
        {
            RuleFor(x => x.NameAz)
                .NotEmpty().WithMessage("Azərbaycanca ölkə adı daxil edilməlidir")
                .MaximumLength(100).WithMessage("Ölkə adı 100 simvoldan çox ola bilməz");
        }
    }

    public class UpdateCountryValidation : AbstractValidator<UpdateCountryDto>
    {
        public UpdateCountryValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Keçərli ID daxil edilməlidir");

            RuleFor(x => x.NameAz)
                .NotEmpty().WithMessage("Azərbaycanca ölkə adı daxil edilməlidir")
                .MaximumLength(100).WithMessage("Ölkə adı 100 simvoldan çox ola bilməz");
        }
    }
}
