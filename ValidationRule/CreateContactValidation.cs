using ApexWebAPI.DTOs.ContactDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class CreateContactValidation : AbstractValidator<CreateContactDto>
    {
        public CreateContactValidation()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon nömrəsi daxil edilməlidir")
                .MaximumLength(20).WithMessage("Telefon nömrəsi 20 simvoldan çox ola bilməz");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail daxil edilməlidir")
                .EmailAddress().WithMessage("Düzgün e-mail formatı daxil edilməlidir");

            RuleFor(x => x.FbUsername)
                .MaximumLength(100).WithMessage("Facebook istifadəçi adı 100 simvoldan çox ola bilməz")
                .When(x => !string.IsNullOrWhiteSpace(x.FbUsername));

            RuleFor(x => x.InstaUsername)
                .MaximumLength(100).WithMessage("Instagram istifadəçi adı 100 simvoldan çox ola bilməz")
                .When(x => !string.IsNullOrWhiteSpace(x.InstaUsername));

            RuleFor(x => x.LnUsername)
                .MaximumLength(100).WithMessage("LinkedIn istifadəçi adı 100 simvoldan çox ola bilməz")
                .When(x => !string.IsNullOrWhiteSpace(x.LnUsername));

            RuleFor(x => x.XUsername)
                .MaximumLength(100).WithMessage("X (Twitter) istifadəçi adı 100 simvoldan çox ola bilməz")
                .When(x => !string.IsNullOrWhiteSpace(x.XUsername));
        }
    }

    public class UpdateContactValidation : AbstractValidator<UpdateContactDto>
    {
        public UpdateContactValidation()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon nömrəsi daxil edilməlidir")
                .MaximumLength(20).WithMessage("Telefon nömrəsi 20 simvoldan çox ola bilməz");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail daxil edilməlidir")
                .EmailAddress().WithMessage("Düzgün e-mail formatı daxil edilməlidir");

            RuleFor(x => x.FbUsername)
                .MaximumLength(100).WithMessage("Facebook istifadəçi adı 100 simvoldan çox ola bilməz")
                .When(x => !string.IsNullOrWhiteSpace(x.FbUsername));

            RuleFor(x => x.InstaUsername)
                .MaximumLength(100).WithMessage("Instagram istifadəçi adı 100 simvoldan çox ola bilməz")
                .When(x => !string.IsNullOrWhiteSpace(x.InstaUsername));

            RuleFor(x => x.LnUsername)
                .MaximumLength(100).WithMessage("LinkedIn istifadəçi adı 100 simvoldan çox ola bilməz")
                .When(x => !string.IsNullOrWhiteSpace(x.LnUsername));

            RuleFor(x => x.XUsername)
                .MaximumLength(100).WithMessage("X (Twitter) istifadəçi adı 100 simvoldan çox ola bilməz")
                .When(x => !string.IsNullOrWhiteSpace(x.XUsername));
        }
    }
}
