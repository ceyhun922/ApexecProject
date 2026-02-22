using ApexWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Concrete
{
    public class ApexDbContext : DbContext
    {
        public ApexDbContext(DbContextOptions<ApexDbContext> options) : base(options) {}

        public DbSet<Hero>? Heroes {get;set;}
        public DbSet<HeroTranslation>? HeroTranslations { get; set; }
        public DbSet<Testimonial>? Testimonials {get;set;}
        public DbSet<TestimonialTranslation>? Translations {get;set;}
        public DbSet<Faq>? Faqs {get;set;}
        public DbSet<FaqTranslation>? FaqTranslations {get;set;}
        public DbSet<About>? Abouts {get;set;}
        public DbSet<AboutTranslation>? AboutTranslations {get;set;}
        public DbSet<Contact>? Contacts {get;set;}
        public DbSet<Country>? Countries {get;set;}
        public DbSet<CountryTranslation>? CountryTranslations {get;set;}
        public DbSet<EducationLevel>? EducationLevels {get;set;}
        public DbSet<EducationLevelTranslation>? EducationLevelTranslations {get;set;}
        public DbSet<AppUser> Users {get;set;}
        public DbSet<Department>? Departments {get;set;}
        public DbSet<DepartmentTranslation>? DepartmentTranslations {get;set;}
        public DbSet<SummerSchool>? SummerSchools { get; set; }
        public DbSet<SummerSchoolTranslation>? SummerSchoolTranslations { get; set; }

    }
}