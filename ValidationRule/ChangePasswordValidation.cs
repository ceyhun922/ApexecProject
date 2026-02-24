using ApexWebAPI.DTOs.ProfileDTOs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.ValidationRule
{
    public class ChangePasswordValidation : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidation(IStringLocalizer<ChangePasswordValidation> localizer)
        {
            RuleFor(p => p.CurrentPassword)
                .NotEmpty().WithMessage(_ => localizer["CurrentPasswordRequired"]);

            RuleFor(p => p.NewPassword)
                .NotEmpty().WithMessage(_ => localizer["NewPasswordRequired"]);
        }
    }
}
