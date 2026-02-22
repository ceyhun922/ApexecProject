using ApexWebAPI.DTOs.AboutDTOs;
using ApexWebAPI.DTOs.ContactDTOs;
using ApexWebAPI.DTOs.CountryDTOs;
using ApexWebAPI.DTOs.DepartmentDTOs;
using ApexWebAPI.DTOs.EducationLevelDTOs;
using ApexWebAPI.DTOs.FaqDTO.cs;
using ApexWebAPI.DTOs.FeatureDTOs;
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
            //Feature
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

            //About
            CreateMap<About, ResultAboutDto>()
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.SubTitle, opt => opt.Ignore());

            CreateMap<About, GetByIdAboutDto>()
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.SubTitle, opt => opt.Ignore());

            CreateMap<CreateAboutDto, About>()
                .ForMember(dest => dest.AboutTranslations, opt => opt.Ignore());

            CreateMap<UpdateAboutDto, About>()
                .ForMember(dest => dest.AboutTranslations, opt => opt.Ignore());

            //Testimonial
            CreateMap<Testimonial, ResultTestimonialDto>()
                .ForMember(dest => dest.Comment, opt => opt.Ignore());

            CreateMap<Testimonial, GetByIdTestimonialDto>()
                .ForMember(dest => dest.Comment, opt => opt.Ignore());

            CreateMap<CreateTestimonialDto, Testimonial>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());

            CreateMap<UpdateTestimonialDto, Testimonial>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore());

            //Faq
            CreateMap<CreateFaqDto, Faq>()
                .ForMember(dest => dest.FaqTranslations, opt => opt.MapFrom(src => new List<FaqTranslation>
                {
                    new FaqTranslation { Language = "az", Title = src.TitleAz, Content = src.ContentAz },
                    new FaqTranslation { Language = "en", Title = src.TitleEn, Content = src.ContentEn },
                    new FaqTranslation { Language = "ru", Title = src.TitleRu, Content = src.ContentRu },
                    new FaqTranslation { Language = "tr", Title = src.TitleTr, Content = src.ContentTr },
                }));

            CreateMap<UpdateFaqDto, Faq>()
                .ForMember(dest => dest.FaqTranslations, opt => opt.MapFrom(src => new List<FaqTranslation>
                {
                        new FaqTranslation { Language = "az", Title = src.TitleAz, Content = src.ContentAz },
                        new FaqTranslation { Language = "en", Title = src.TitleEn, Content = src.ContentEn },
                        new FaqTranslation { Language = "ru", Title = src.TitleRu, Content = src.ContentRu },
                        new FaqTranslation { Language = "tr", Title = src.TitleTr, Content = src.ContentTr }
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

            //Country
            CreateMap<Country, ResultCountryDto>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                   src.CountryTranslations.FirstOrDefault(t => t.Language == "az").Name ?? string.Empty));

            CreateMap<Country, GetByIdCountryDto>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                   src.CountryTranslations.FirstOrDefault(t => t.Language == "az").Name ?? string.Empty));

            CreateMap<UpdateCountryDto, Country>()
                .ForMember(dest => dest.CountryTranslations, opt => opt.MapFrom(src => new List<CountryTranslation>
                {
                        new CountryTranslation { Language = "az",  Name= src.NameAz},
                        new CountryTranslation { Language = "en", Name = src.NameEn},
                        new CountryTranslation { Language = "ru", Name = src.NameRu},
                        new CountryTranslation { Language = "tr", Name = src.NameTr}
                }));

            CreateMap<CreateCountryDto, Country>()
               .ForMember(dest => dest.CountryTranslations, opt => opt.MapFrom(src => new List<CountryTranslation>
               {
                    new CountryTranslation { Language = "az",  Name = src.NameAz },
                    new CountryTranslation { Language = "en",  Name = src.NameEn },
                    new CountryTranslation { Language = "ru",  Name = src.NameRu },
                    new CountryTranslation { Language = "tr",  Name = src.NameTr },
               }));

            //EducationLevel
            CreateMap<EducationLevel, ResultEducationLevelDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                src.EducationLevelTranslations.FirstOrDefault(t => t.Language == "az").Name ?? string.Empty));

            CreateMap<EducationLevel, GetByIdEducationLevelDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                  src.EducationLevelTranslations.FirstOrDefault(t => t.Language == "az").Name ?? string.Empty));

            CreateMap<UpdateEducationLevelDto, EducationLevel>()
                .ForMember(dest => dest.EducationLevelTranslations, opt => opt.MapFrom(src => new List<EducationLevelTranslation>
                {
                        new EducationLevelTranslation { Language = "az",  Name= src.NameAz},
                        new EducationLevelTranslation { Language = "en", Name = src.NameEn},
                        new EducationLevelTranslation { Language = "ru", Name = src.NameRu},
                        new EducationLevelTranslation { Language = "tr", Name = src.NameTr}
                }));

            CreateMap<CreateEducationLevelDto, EducationLevel>()
               .ForMember(dest => dest.EducationLevelTranslations, opt => opt.MapFrom(src => new List<EducationLevelTranslation>
               {
                    new EducationLevelTranslation { Language = "az",  Name = src.NameAz },
                    new EducationLevelTranslation { Language = "en",  Name = src.NameEn },
                    new EducationLevelTranslation { Language = "ru",  Name = src.NameRu },
                    new EducationLevelTranslation { Language = "tr",  Name = src.NameTr },
               }));
            
            //Department
            CreateMap<Department, ResultDepartmentDto>()
                .ForMember(dest =>dest.Name, opt => opt.MapFrom(src =>
                src.DepartmentTranslations.FirstOrDefault(d=>d.Language =="az").Name ?? string.Empty ));

            CreateMap<Department, GetByIdDepartmentDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                  src.DepartmentTranslations.FirstOrDefault(t => t.Language == "az").Name ?? string.Empty));

            CreateMap<UpdateDepartmentDto, Department>()
                .ForMember(dest => dest.DepartmentTranslations, opt => opt.MapFrom(src => new List<DepartmentTranslation>
                {
                        new DepartmentTranslation { Language = "az",  Name= src.NameAz},
                        new DepartmentTranslation { Language = "en", Name = src.NameEn},
                        new DepartmentTranslation { Language = "ru", Name = src.NameRu},
                        new DepartmentTranslation { Language = "tr", Name = src.NameTr}
                }));
            
             CreateMap<CreateDepartmentDto, Department>()
               .ForMember(dest => dest.DepartmentTranslations, opt => opt.MapFrom(src => new List<DepartmentTranslation>
               {
                    new DepartmentTranslation { Language = "az",  Name = src.NameAz },
                    new DepartmentTranslation { Language = "en",  Name = src.NameEn },
                    new DepartmentTranslation { Language = "ru",  Name = src.NameRu },
                    new DepartmentTranslation { Language = "tr",  Name = src.NameTr },
               }));

            //Information

            CreateMap<CreateInformationDto, Contact>().ReverseMap();
            CreateMap<Contact, ResultInformationDto>().ReverseMap();
            CreateMap<Contact, UpdateInformationDto>().ReverseMap();
            CreateMap<Contact, GetByIdInformationDto>().ReverseMap();

            //Message

            CreateMap<CreateMessageDto, Contact>().ReverseMap();
            CreateMap<Contact, ResultMessageDto>().ReverseMap();
            CreateMap<Contact, UpdateMessageDto>().ReverseMap();
            CreateMap<Contact, GetByIdMessageDto>().ReverseMap();

            //Contact
            CreateMap<CreateContactDto, Contact>().ReverseMap();
            CreateMap<Contact, ResultContactDto>().ReverseMap();
            CreateMap<Contact, UpdateContactDto>().ReverseMap();
            CreateMap<Contact, GetByIdContactDto>().ReverseMap();




        }
    }
}