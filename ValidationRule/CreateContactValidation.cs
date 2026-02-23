using ApexWebAPI.DTOs.ContactInfoDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class CreateContactValidation : AbstractValidator<CreateContactInfoDto>
    {
        public CreateContactValidation()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon nömrəsi daxil edilməlidir")
                .MaximumLength(20).WithMessage("Telefon nömrəsi 20 simvoldan çox ola bilməz");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail daxil edilməlidir")
                .EmailAddress().WithMessage("Düzgün e-mail formatı daxil edilməlidir");

        }
    }

    public class UpdateContactValidation : AbstractValidator<UpdateContactInfoDto>
    {
        public UpdateContactValidation()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon nömrəsi daxil edilməlidir")
                .MaximumLength(20).WithMessage("Telefon nömrəsi 20 simvoldan çox ola bilməz");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail daxil edilməlidir")
                .EmailAddress().WithMessage("Düzgün e-mail formatı daxil edilməlidir");

           
        }
    }
}
