using ApexWebAPI.DTOs.ProfileDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class UserProfileValidation : AbstractValidator<UserProfileDto>
    {
        public UserProfileValidation()
        {
            RuleFor(u=>u.FullName).Empty().WithMessage("İstifadeçi Adı Soyadı Girmediniz");
            RuleFor(u =>u.Username).Empty().WithMessage("İstifadeçi Adı Girmediniz");
            RuleFor(u=>u.Email).Empty().WithMessage("İstifadeçi Mail Girmediniz");

        }
    }
}