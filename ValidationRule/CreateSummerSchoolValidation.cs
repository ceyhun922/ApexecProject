using ApexWebAPI.DTOs.SummerSchoolDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class CreateSummerSchoolValidation : AbstractValidator<CreateSummerSchoolDto>
    {
        public CreateSummerSchoolValidation()
        {
            RuleFor(x => x.TitleAz)
                .NotEmpty().WithMessage("Azərbaycanca başlıq daxil edilməlidir")
                .MaximumLength(300).WithMessage("Başlıq 300 simvoldan çox ola bilməz");

            RuleFor(x => x.SubTitleAz)
                .MaximumLength(500).WithMessage("Alt başlıq 500 simvoldan çox ola bilməz");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("Ölkə seçilməlidir");
        }
    }

    public class UpdateSummerSchoolValidation : AbstractValidator<UpdateSummerSchoolDto>
    {
        public UpdateSummerSchoolValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Keçərli ID daxil edilməlidir");

            RuleFor(x => x.TitleAz)
                .NotEmpty().WithMessage("Azərbaycanca başlıq daxil edilməlidir")
                .MaximumLength(300).WithMessage("Başlıq 300 simvoldan çox ola bilməz");

            RuleFor(x => x.SubTitleAz)
                .MaximumLength(500).WithMessage("Alt başlıq 500 simvoldan çox ola bilməz");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("Ölkə seçilməlidir");
        }
    }
}
