using ApexWebAPI.DTOs.AboutDTOs;
using ApexWebAPI.DTOs.ContactDTOs;
using ApexWebAPI.DTOs.CountryDTOs;
using ApexWebAPI.DTOs.DepartmentDTOs;
using ApexWebAPI.DTOs.EducationLevelDTOs;
using ApexWebAPI.DTOs.FaqDTO.cs;
using ApexWebAPI.DTOs.FeatureDTOs;
using ApexWebAPI.DTOs.FooterDTOs;
using ApexWebAPI.DTOs.InformationDTOs;
using ApexWebAPI.DTOs.MessageDTOs;
using ApexWebAPI.DTOs.MessageDTOs.cs;
using ApexWebAPI.DTOs.TestimonialDTOs;
using ApexWebAPI.Entities;
using AutoMapper;

namespace ApexWebAPI.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            // Hero
            CreateMap<Hero, ResultHeroDto>()
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.SubTitle, opt => opt.Ignore());

            CreateMap<Hero, GetByIdHeroDto>()
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.SubTitle, opt => opt.Ignore());

            CreateMap<CreateHeroDto, Hero>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());

            CreateMap<UpdateHeroDto, Hero>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());

            // About
 CreateMap<About, ResultAboutDto>()
    .ForMember(dest => dest.Title, opt => opt.Ignore())
    .ForMember(dest => dest.SubTitle, opt => opt.Ignore())
    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

CreateMap<About, GetByIdAboutDto>()
    .ForMember(dest => dest.Title, opt => opt.Ignore())
    .ForMember(dest => dest.SubTitle, opt => opt.Ignore())
    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

            CreateMap<CreateAboutDto, About>()
                .ForMember(dest => dest.AboutTranslations, opt => opt.Ignore());

            CreateMap<UpdateAboutDto, About>()
                .ForMember(dest => dest.AboutTranslations, opt => opt.Ignore());

            // Testimonial
            CreateMap<Testimonial, ResultTestimonialDto>()
      .ForMember(dest => dest.Comment, opt => opt.Ignore())
      .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

            CreateMap<Testimonial, GetByIdTestimonialDto>()
                .ForMember(dest => dest.Comment, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

            CreateMap<CreateTestimonialDto, Testimonial>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());

            CreateMap<UpdateTestimonialDto, Testimonial>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());

            // Faq
            CreateMap<CreateFaqDto, Faq>()
                .ForMember(dest => dest.FaqTranslations, opt => opt.MapFrom(src => new List<FaqTranslation>
                {
                    new() { Language = "az", Title = src.TitleAz, Content = src.ContentAz },
                    new() { Language = "en", Title = src.TitleEn, Content = src.ContentEn },
                    new() { Language = "ru", Title = src.TitleRu, Content = src.ContentRu },
                    new() { Language = "tr", Title = src.TitleTr, Content = src.ContentTr },
                }));

            CreateMap<UpdateFaqDto, Faq>()
                .ForMember(dest => dest.FaqTranslations, opt => opt.MapFrom(src => new List<FaqTranslation>
                {
                    new() { Language = "az", Title = src.TitleAz, Content = src.ContentAz },
                    new() { Language = "en", Title = src.TitleEn, Content = src.ContentEn },
                    new() { Language = "ru", Title = src.TitleRu, Content = src.ContentRu },
                    new() { Language = "tr", Title = src.TitleTr, Content = src.ContentTr },
                }));

            CreateMap<Faq, ResultFaqDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src =>
                    src.FaqTranslations.FirstOrDefault(t => t.Language == "az").Title ?? string.Empty))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src =>
                    src.FaqTranslations.FirstOrDefault(t => t.Language == "az").Content ?? string.Empty));

            CreateMap<Faq, GetByIdFaqDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src =>
                    src.FaqTranslations.FirstOrDefault(t => t.Language == "az").Title ?? string.Empty))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src =>
                    src.FaqTranslations.FirstOrDefault(t => t.Language == "az").Content ?? string.Empty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            // Country
            CreateMap<Country, ResultCountryDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    src.CountryTranslations.FirstOrDefault(t => t.Language == "az").Name ?? string.Empty));

            CreateMap<Country, GetByIdCountryDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    src.CountryTranslations.FirstOrDefault(t => t.Language == "az").Name ?? string.Empty));

            CreateMap<CreateCountryDto, Country>()
                .ForMember(dest => dest.CountryTranslations, opt => opt.MapFrom(src => new List<CountryTranslation>
                {
                    new() { Language = "az", Name = src.NameAz },
                    new() { Language = "en", Name = src.NameEn },
                    new() { Language = "ru", Name = src.NameRu },
                    new() { Language = "tr", Name = src.NameTr },
                }));

            CreateMap<UpdateCountryDto, Country>()
                .ForMember(dest => dest.CountryTranslations, opt => opt.MapFrom(src => new List<CountryTranslation>
                {
                    new() { Language = "az", Name = src.NameAz },
                    new() { Language = "en", Name = src.NameEn },
                    new() { Language = "ru", Name = src.NameRu },
                    new() { Language = "tr", Name = src.NameTr },
                }));

            // EducationLevel
            CreateMap<EducationLevel, ResultEducationLevelDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    src.EducationLevelTranslations.FirstOrDefault(t => t.Language == "az").Name ?? string.Empty));

            CreateMap<EducationLevel, GetByIdEducationLevelDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    src.EducationLevelTranslations.FirstOrDefault(t => t.Language == "az").Name ?? string.Empty));

            CreateMap<CreateEducationLevelDto, EducationLevel>()
                .ForMember(dest => dest.EducationLevelTranslations, opt => opt.MapFrom(src => new List<EducationLevelTranslation>
                {
                    new() { Language = "az", Name = src.NameAz },
                    new() { Language = "en", Name = src.NameEn },
                    new() { Language = "ru", Name = src.NameRu },
                    new() { Language = "tr", Name = src.NameTr },
                }));

            CreateMap<UpdateEducationLevelDto, EducationLevel>()
                .ForMember(dest => dest.EducationLevelTranslations, opt => opt.MapFrom(src => new List<EducationLevelTranslation>
                {
                    new() { Language = "az", Name = src.NameAz },
                    new() { Language = "en", Name = src.NameEn },
                    new() { Language = "ru", Name = src.NameRu },
                    new() { Language = "tr", Name = src.NameTr },
                }));

            // Department
            CreateMap<Department, ResultDepartmentDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    src.DepartmentTranslations.FirstOrDefault(d => d.Language == "az").Name ?? string.Empty));

            CreateMap<Department, GetByIdDepartmentDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    src.DepartmentTranslations.FirstOrDefault(t => t.Language == "az").Name ?? string.Empty));

            CreateMap<CreateDepartmentDto, Department>()
                .ForMember(dest => dest.DepartmentTranslations, opt => opt.MapFrom(src => new List<DepartmentTranslation>
                {
                    new() { Language = "az", Name = src.NameAz },
                    new() { Language = "en", Name = src.NameEn },
                    new() { Language = "ru", Name = src.NameRu },
                    new() { Language = "tr", Name = src.NameTr },
                }));

            CreateMap<UpdateDepartmentDto, Department>()
                .ForMember(dest => dest.DepartmentTranslations, opt => opt.MapFrom(src => new List<DepartmentTranslation>
                {
                    new() { Language = "az", Name = src.NameAz },
                    new() { Language = "en", Name = src.NameEn },
                    new() { Language = "ru", Name = src.NameRu },
                    new() { Language = "tr", Name = src.NameTr },
                }));

            // Information
            CreateMap<CreateInformationDto, Contact>().ReverseMap();
            CreateMap<Contact, ResultInformationDto>().ReverseMap();
            CreateMap<Contact, UpdateInformationDto>().ReverseMap();
            CreateMap<Contact, GetByIdInformationDto>().ReverseMap();

            // Message
            CreateMap<CreateMessageDto, Contact>().ReverseMap();
            CreateMap<Contact, ResultMessageDto>().ReverseMap();
            CreateMap<Contact, UpdateMessageDto>().ReverseMap();
            CreateMap<Contact, GetByIdMessageDto>().ReverseMap();

            // Contact
            CreateMap<CreateContactDto, Contact>().ReverseMap();

         CreateMap<Contact, ResultContactDto>()
    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

CreateMap<Contact, GetByIdContactDto>()
    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

            CreateMap<Contact, UpdateContactDto>().ReverseMap();

            // Footer
            CreateMap<Contact, ResultFooterDto>().ReverseMap();
            CreateMap<Contact, UpdateFooterDto>().ReverseMap();
        }
    }
}