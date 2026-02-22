using ApexWebAPI.DTOs.ProfileDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class ChangePasswordValidation : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidation()
        {
            RuleFor(p =>p.CurrentPassword).NotEmpty().WithMessage("Cari şifrə Girilməlidir");

            RuleFor(p=>p.NewPassword).NotEmpty().WithMessage("Yeni Şifrə Girilmelidir");
        }
    }
}