using ApexWebAPI.DTOs.FeatureDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class CreateHeroValidation : AbstractValidator<CreateHeroDto>
    {
        public CreateHeroValidation()
        {
            RuleFor(x => x.TitleAz)
                .NotEmpty().WithMessage("Azərbaycanca başlıq daxil edilməlidir")
                .MaximumLength(200).WithMessage("Başlıq 200 simvoldan çox ola bilməz");

            RuleFor(x => x.TitleEn)
                .MaximumLength(200).WithMessage("İngilis başlığı 200 simvoldan çox ola bilməz");

            RuleFor(x => x.TitleRu)
                .MaximumLength(200).WithMessage("Rus başlığı 200 simvoldan çox ola bilməz");

            RuleFor(x => x.TitleTr)
                .MaximumLength(200).WithMessage("Türk başlığı 200 simvoldan çox ola bilməz");

            RuleFor(x => x.Video)
                .Must(file => file == null || file.Length <= 52_428_800)
                .WithMessage("Video fayl 50MB-dan böyük ola bilməz");
        }
    }

    public class UpdateHeroValidation : AbstractValidator<UpdateHeroDto>
    {
        public UpdateHeroValidation()
        {
            RuleFor(x => x.TitleAz)
                .NotEmpty().WithMessage("Azərbaycanca başlıq daxil edilməlidir")
                .MaximumLength(200).WithMessage("Başlıq 200 simvoldan çox ola bilməz");

            RuleFor(x => x.Video)
                .Must(file => file == null || file.Length <= 52_428_800)
                .WithMessage("Video fayl 50MB-dan böyük ola bilməz");
        }
    }
}
