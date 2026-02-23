using ApexWebAPI.DTOs.FaqDTO.cs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class CreateFaqValidation : AbstractValidator<CreateFaqDto>
    {
        public CreateFaqValidation()
        {
            RuleFor(x => x.TitleAz)
                .NotEmpty().WithMessage("Azərbaycanca sual daxil edilməlidir")
                .MaximumLength(300).WithMessage("Sual 300 simvoldan çox ola bilməz");

            RuleFor(x => x.ContentAz)
                .NotEmpty().WithMessage("Azərbaycanca cavab daxil edilməlidir");
        }
    }

    public class UpdateFaqValidation : AbstractValidator<UpdateFaqDto>
    {
        public UpdateFaqValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Keçərli ID daxil edilməlidir");

            RuleFor(x => x.TitleAz)
                .NotEmpty().WithMessage("Azərbaycanca sual daxil edilməlidir")
                .MaximumLength(300).WithMessage("Sual 300 simvoldan çox ola bilməz");

            RuleFor(x => x.ContentAz)
                .NotEmpty().WithMessage("Azərbaycanca cavab daxil edilməlidir");
        }
    }
}
