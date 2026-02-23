using ApexWebAPI.DTOs.TestimonialDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class CreateTestimonialValidation : AbstractValidator<CreateTestimonialDto>
    {
        public CreateTestimonialValidation()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Ad Soyad daxil edilməlidir")
                .MaximumLength(100).WithMessage("Ad Soyad 100 simvoldan çox ola bilməz");

            RuleFor(x => x.CommentAz)
                .NotEmpty().WithMessage("Azərbaycanca rəy daxil edilməlidir");
        }
    }

    public class UpdateTestimonialValidation : AbstractValidator<UpdateTestimonialDto>
    {
        public UpdateTestimonialValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Keçərli ID daxil edilməlidir");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Ad Soyad daxil edilməlidir")
                .MaximumLength(100).WithMessage("Ad Soyad 100 simvoldan çox ola bilməz");

            RuleFor(x => x.CommentAz)
                .NotEmpty().WithMessage("Azərbaycanca rəy daxil edilməlidir");
        }
    }
}
