using ApexWebAPI.DTOs.InformationDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class CreateInformationValidation : AbstractValidator<CreateInformationDto>
    {
        public CreateInformationValidation()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Ad Soyad daxil edilməlidir")
                .MaximumLength(100).WithMessage("Ad Soyad 100 simvoldan çox ola bilməz");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon nömrəsi daxil edilməlidir")
                .MaximumLength(20).WithMessage("Telefon nömrəsi 20 simvoldan çox ola bilməz");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Düzgün e-mail formatı daxil edilməlidir");

            RuleFor(x => x.Education)
                .NotEmpty().WithMessage("Təhsil məlumatı daxil edilməlidir")
                .MaximumLength(200).WithMessage("Təhsil məlumatı 200 simvoldan çox ola bilməz");

            RuleFor(x => x.ClassOrYear)
                .NotEmpty().WithMessage("Sinif/il məlumatı daxil edilməlidir")
                .MaximumLength(20).WithMessage("Sinif/il 20 simvoldan çox ola bilməz");
        }
    }

    public class UpdateInformationValidation : AbstractValidator<UpdateInformationDto>
    {
        public UpdateInformationValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Keçərli ID daxil edilməlidir");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Ad Soyad daxil edilməlidir")
                .MaximumLength(100).WithMessage("Ad Soyad 100 simvoldan çox ola bilməz");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon nömrəsi daxil edilməlidir")
                .MaximumLength(20).WithMessage("Telefon nömrəsi 20 simvoldan çox ola bilməz");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Düzgün e-mail formatı daxil edilməlidir");

            RuleFor(x => x.Education)
                .NotEmpty().WithMessage("Təhsil məlumatı daxil edilməlidir")
                .MaximumLength(200).WithMessage("Təhsil məlumatı 200 simvoldan çox ola bilməz");

            RuleFor(x => x.ClassOrYear)
                .NotEmpty().WithMessage("Sinif/il məlumatı daxil edilməlidir")
                .MaximumLength(20).WithMessage("Sinif/il 20 simvoldan çox ola bilməz");
        }
    }
}
