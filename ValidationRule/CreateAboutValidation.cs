using ApexWebAPI.DTOs.AboutDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class CreateAboutValidation : AbstractValidator<CreateAboutDto>
    {
        public CreateAboutValidation()
        {
            RuleFor(x => x.TitleAz)
                .NotEmpty().WithMessage("Azərbaycanca başlıq daxil edilməlidir")
                .MaximumLength(300).WithMessage("Başlıq 300 simvoldan çox ola bilməz");

            RuleFor(x => x.SubTitleAz)
                .NotEmpty().WithMessage("Azərbaycanca alt başlıq daxil edilməlidir")
                .MaximumLength(500).WithMessage("Alt başlıq 500 simvoldan çox ola bilməz");
        }
    }

    public class UpdateAboutValidation : AbstractValidator<UpdateAboutDto>
    {
        public UpdateAboutValidation()
        {
            RuleFor(x => x.TitleAz)
                .NotEmpty().WithMessage("Azərbaycanca başlıq daxil edilməlidir")
                .MaximumLength(300).WithMessage("Başlıq 300 simvoldan çox ola bilməz");

            RuleFor(x => x.SubTitleAz)
                .NotEmpty().WithMessage("Azərbaycanca alt başlıq daxil edilməlidir")
                .MaximumLength(500).WithMessage("Alt başlıq 500 simvoldan çox ola bilməz");
        }
    }
}
