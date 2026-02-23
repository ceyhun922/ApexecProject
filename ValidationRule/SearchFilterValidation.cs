using ApexWebAPI.DTOs.SearchDTOs;
using FluentValidation;

namespace ApexWebAPI.ValidationRule
{
    public class SearchFilterValidation : AbstractValidator<SearchFilterDto>
    {
        public SearchFilterValidation()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Səhifə nömrəsi 0-dan böyük olmalıdır");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Səhifə ölçüsü 1 ilə 100 arasında olmalıdır");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).When(x => x.CountryId.HasValue)
                .WithMessage("Ölkə ID 0-dan böyük olmalıdır");

            RuleFor(x => x.EducationLevelId)
                .GreaterThan(0).When(x => x.EducationLevelId.HasValue)
                .WithMessage("Təhsil səviyyəsi ID 0-dan böyük olmalıdır");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).When(x => x.DepartmentId.HasValue)
                .WithMessage("Fakültə ID 0-dan böyük olmalıdır");
        }
    }
}
