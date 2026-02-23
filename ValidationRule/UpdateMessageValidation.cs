using ApexWebAPI.DTOs.MessageDTOs.cs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class UpdateMessageValidation : AbstractValidator<UpdateMessageDto>
    {
        public UpdateMessageValidation()
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

            RuleFor(x => x.Messagee)
                .NotEmpty().WithMessage("Mesaj daxil edilməlidir")
                .MaximumLength(1000).WithMessage("Mesaj 1000 simvoldan çox ola bilməz");
        }
    }
}
