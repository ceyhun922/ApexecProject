using ApexWebAPI.DTOs.LoginDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class LoginValidation : AbstractValidator<LoginDto>
    {
        public LoginValidation()
        {
            RuleFor(l => l.Username).NotEmpty().WithMessage("İstifadeçi Adı Boş Ola Bilmez");
            RuleFor(l => l.Password).NotEmpty().WithMessage("Şifrə Boş Ola Bilməz");

        }
    }
}