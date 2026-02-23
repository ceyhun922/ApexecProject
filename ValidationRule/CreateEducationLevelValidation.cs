using ApexWebAPI.DTOs.EducationLevelDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class CreateEducationLevelValidation : AbstractValidator<CreateEducationLevelDto>
    {
        public CreateEducationLevelValidation()
        {
            RuleFor(x => x.NameAz)
                .NotEmpty().WithMessage("Azərbaycanca təhsil səviyyəsi adı daxil edilməlidir")
                .MaximumLength(200).WithMessage("Təhsil səviyyəsi adı 200 simvoldan çox ola bilməz");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("Ölkə seçilməlidir");
        }
    }

    public class UpdateEducationLevelValidation : AbstractValidator<UpdateEducationLevelDto>
    {
        public UpdateEducationLevelValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Keçərli ID daxil edilməlidir");

            RuleFor(x => x.NameAz)
                .NotEmpty().WithMessage("Azərbaycanca təhsil səviyyəsi adı daxil edilməlidir")
                .MaximumLength(200).WithMessage("Təhsil səviyyəsi adı 200 simvoldan çox ola bilməz");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("Ölkə seçilməlidir");
        }
    }
}
