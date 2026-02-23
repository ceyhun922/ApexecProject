using ApexWebAPI.DTOs.FooterDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class UpdateFooterValidation : AbstractValidator<UpdateFooterDto>
    {
        public UpdateFooterValidation()
        {
            RuleFor(x => x.FbUrl)
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .When(x => !string.IsNullOrWhiteSpace(x.FbUrl))
                .WithMessage("Facebook URL düzgün formatda deyil");

            RuleFor(x => x.InstaUrl)
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .When(x => !string.IsNullOrWhiteSpace(x.InstaUrl))
                .WithMessage("Instagram URL düzgün formatda deyil");

            RuleFor(x => x.LnUrl)
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .When(x => !string.IsNullOrWhiteSpace(x.LnUrl))
                .WithMessage("LinkedIn URL düzgün formatda deyil");

            RuleFor(x => x.XUrl)
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .When(x => !string.IsNullOrWhiteSpace(x.XUrl))
                .WithMessage("X (Twitter) URL düzgün formatda deyil");

            RuleFor(x => x.OtherUrl)
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .When(x => !string.IsNullOrWhiteSpace(x.OtherUrl))
                .WithMessage("URL düzgün formatda deyil");
        }
    }
}
