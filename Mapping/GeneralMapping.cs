using ApexWebAPI.DTOs.AboutDTOs;
using ApexWebAPI.DTOs.ContactHeaderDTOs;
using ApexWebAPI.DTOs.HomeVideoSectionDTOs;
using ApexWebAPI.DTOs.AboutVideoSectionDTOs;
using ApexWebAPI.DTOs.AboutCounterDTOs;
using ApexWebAPI.DTOs.StatisticDTOs;
using ApexWebAPI.DTOs.CountryDTOs;
using ApexWebAPI.DTOs.DepartmentDTOs;
using ApexWebAPI.DTOs.EducationLevelDTOs;
using ApexWebAPI.DTOs.FaqDTO.cs;
using ApexWebAPI.DTOs.FeatureDTOs;
using ApexWebAPI.DTOs.FooterDTOs;
using ApexWebAPI.DTOs.InformationDTOs;
using ApexWebAPI.DTOs.MessageDTOs;
using ApexWebAPI.DTOs.MessageDTOs.cs;
using ApexWebAPI.DTOs.SummerSchoolDTOs;
using ApexWebAPI.DTOs.TestimonialDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using ApexWebAPI.DTOs.ContactInfoDTOs;

namespace ApexWebAPI.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            // Hero
            CreateMap<Hero, ResultHeroDto>()
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.SubTitle, opt => opt.Ignore())
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.VideoUrl));

            CreateMap<Hero, GetByIdHeroDto>()
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.SubTitle, opt => opt.Ignore())
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.VideoUrl));

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

            //SummerSchool
            CreateMap<CreateSummerSchoolDto, SummerSchool>()
             .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => new List<SummerSchoolTranslation>
             {
                    new() { Language = "az", Title = src.TitleAz, SubTitle = src.SubTitleAz },
                    new() { Language = "en", Title = src.TitleEn, SubTitle = src.SubTitleEn },
                    new() { Language = "ru", Title = src.TitleRu, SubTitle = src.SubTitleRu },
                    new() { Language = "tr", Title = src.TitleTr, SubTitle = src.SubTitleTr },
             }));

            CreateMap<UpdateSummerSchoolDto, SummerSchool>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => new List<SummerSchoolTranslation>
                {
                    new() { Language = "az", Title = src.TitleAz, SubTitle = src.SubTitleAz },
                    new() { Language = "en", Title = src.TitleEn, SubTitle = src.SubTitleEn },
                    new() { Language = "ru", Title = src.TitleRu, SubTitle = src.SubTitleRu },
                    new() { Language = "tr", Title = src.TitleTr, SubTitle = src.SubTitleTr },
                }));

            CreateMap<SummerSchool, ResultSummerSchoolDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src =>
                    src.Translations.FirstOrDefault(t => t.Language == "az").Title ?? string.Empty))
                .ForMember(dest => dest.SubTitle, opt => opt.MapFrom(src =>
                    src.Translations.FirstOrDefault(t => t.Language == "az").SubTitle ?? string.Empty));

            CreateMap<SummerSchool, GetByIdSchoolSummerDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src =>
                    src.Translations.FirstOrDefault(t => t.Language == "az").Title ?? string.Empty))
                .ForMember(dest => dest.SubTitle, opt => opt.MapFrom(src =>
                    src.Translations.FirstOrDefault(t => t.Language == "az").SubTitle ?? string.Empty))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

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
            CreateMap<CreateInformationDto, Information>().ReverseMap();
            CreateMap<Information, ResultInformationDto>().ReverseMap();
            CreateMap<Information, UpdateInformationDto>().ReverseMap();
            CreateMap<Information, GetByIdInformationDto>().ReverseMap();

            // Message
            CreateMap<CreateMessageDto, Message>().ReverseMap();
            CreateMap<Message, ResultMessageDto>().ReverseMap();
            CreateMap<Message, UpdateMessageDto>().ReverseMap();
            CreateMap<Message, GetByIdMessageDto>().ReverseMap();

            // ContactInfo
            CreateMap<CreateContactInfoDto, ContactInfo>().ReverseMap();
            CreateMap<UpdateContactInfoDto, ContactInfo>().ReverseMap();
            CreateMap<ContactInfo, ResultContactInfoDto>().ReverseMap();
            CreateMap<ContactInfo, GetByIdContactDto>().ReverseMap();

            // Footer
            CreateMap<Footer, ResultFooterDto>().ReverseMap();
            CreateMap<Footer, UpdateFooterDto>().ReverseMap();
            CreateMap<CreateFooterDto, Footer>().ReverseMap();
            // Statistic
            CreateMap<Statistic, ResultStatisticDto>()
                .ForMember(dest => dest.Text1, opt => opt.Ignore())
                .ForMember(dest => dest.Text2, opt => opt.Ignore())
                .ForMember(dest => dest.Text3, opt => opt.Ignore())
                .ForMember(dest => dest.Text4, opt => opt.Ignore());

            CreateMap<Statistic, GetByIdStatisticDto>()
                .ForMember(dest => dest.Text1, opt => opt.Ignore())
                .ForMember(dest => dest.Text2, opt => opt.Ignore())
                .ForMember(dest => dest.Text3, opt => opt.Ignore())
                .ForMember(dest => dest.Text4, opt => opt.Ignore());

            CreateMap<CreateStatisticDto, Statistic>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());

            CreateMap<UpdateStatisticDto, Statistic>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());

            // HomeVideoSection
            CreateMap<HomeVideoSection, ResultHomeVideoSectionDto>()
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.SubTitle, opt => opt.Ignore());

            CreateMap<HomeVideoSection, GetByIdHomeVideoSectionDto>()
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.SubTitle, opt => opt.Ignore());

            CreateMap<CreateHomeVideoSectionDto, HomeVideoSection>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());

            CreateMap<UpdateHomeVideoSectionDto, HomeVideoSection>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());

            // AboutVideoSection
            CreateMap<AboutVideoSection, ResultAboutVideoSectionDto>()
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.SubTitle, opt => opt.Ignore());

            CreateMap<AboutVideoSection, GetByIdAboutVideoSectionDto>()
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.SubTitle, opt => opt.Ignore());

            CreateMap<CreateAboutVideoSectionDto, AboutVideoSection>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());

            CreateMap<UpdateAboutVideoSectionDto, AboutVideoSection>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());

            // AboutCounter
            CreateMap<AboutCounter, ResultAboutCounterDto>()
                .ForMember(dest => dest.Text1, opt => opt.Ignore())
                .ForMember(dest => dest.Text2, opt => opt.Ignore())
                .ForMember(dest => dest.Text3, opt => opt.Ignore())
                .ForMember(dest => dest.Text4, opt => opt.Ignore());

            CreateMap<AboutCounter, GetByIdAboutCounterDto>()
                .ForMember(dest => dest.Text1, opt => opt.Ignore())
                .ForMember(dest => dest.Text2, opt => opt.Ignore())
                .ForMember(dest => dest.Text3, opt => opt.Ignore())
                .ForMember(dest => dest.Text4, opt => opt.Ignore());

            CreateMap<CreateAboutCounterDto, AboutCounter>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());

            CreateMap<UpdateAboutCounterDto, AboutCounter>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());

            // ContactHeader
            CreateMap<ContactHeader, ResultContactHeaderDto>()
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.SubTitle, opt => opt.Ignore());
            CreateMap<ContactHeader, GetByIdContactHeaderDto>()
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.SubTitle, opt => opt.Ignore());
            CreateMap<CreateContactHeaderDto, ContactHeader>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());
            CreateMap<UpdateContactHeaderDto, ContactHeader>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());
        }
    }
}