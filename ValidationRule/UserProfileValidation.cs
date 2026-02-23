using ApexWebAPI.DTOs.ProfileDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class UserProfileValidation : AbstractValidator<UserProfileDto>
    {
        public UserProfileValidation()
        {
            RuleFor(u => u.FullName).NotEmpty().WithMessage("İstifadəçi Adı Soyadı Daxil Edilməlidir");
            RuleFor(u => u.Username).NotEmpty().WithMessage("İstifadəçi Adı Daxil Edilməlidir");
            RuleFor(u => u.Email).NotEmpty().EmailAddress().WithMessage("Düzgün E-mail Daxil Edilməlidir");

        }
    }
}