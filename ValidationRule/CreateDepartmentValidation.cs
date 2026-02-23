using ApexWebAPI.DTOs.DepartmentDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class CreateDepartmentValidation : AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentValidation()
        {
            RuleFor(x => x.NameAz)
                .NotEmpty().WithMessage("Azərbaycanca fakültə adı daxil edilməlidir")
                .MaximumLength(200).WithMessage("Fakültə adı 200 simvoldan çox ola bilməz");

            RuleFor(x => x.EducationLevelId)
                .GreaterThan(0).WithMessage("Təhsil səviyyəsi seçilməlidir");
        }
    }

    public class UpdateDepartmentValidation : AbstractValidator<UpdateDepartmentDto>
    {
        public UpdateDepartmentValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Keçərli ID daxil edilməlidir");

            RuleFor(x => x.NameAz)
                .NotEmpty().WithMessage("Azərbaycanca fakültə adı daxil edilməlidir")
                .MaximumLength(200).WithMessage("Fakültə adı 200 simvoldan çox ola bilməz");

            RuleFor(x => x.EducationLevelId)
                .GreaterThan(0).WithMessage("Təhsil səviyyəsi seçilməlidir");
        }
    }
}
