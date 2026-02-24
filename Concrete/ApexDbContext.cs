using ApexWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ApexWebAPI.Concrete
{
    public class ApexDbContextFactory : IDesignTimeDbContextFactory<ApexDbContext>
    {
        public ApexDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApexDbContext>();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            return new ApexDbContext(optionsBuilder.Options);
        }
    }

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
        public DbSet<Country>? Countries {get;set;}
        public DbSet<CountryTranslation>? CountryTranslations {get;set;}
        public DbSet<EducationLevel>? EducationLevels {get;set;}
        public DbSet<EducationLevelTranslation>? EducationLevelTranslations {get;set;}
        public DbSet<AppUser> Users {get;set;}
        public DbSet<Department>? Departments {get;set;}
        public DbSet<DepartmentTranslation>? DepartmentTranslations {get;set;}
        public DbSet<SummerSchool>? SummerSchools { get; set; }
        public DbSet<SummerSchoolTranslation>? SummerSchoolTranslations { get; set; }
        public DbSet<HomeVideoSection>? HomeVideoSections { get; set; }
        public DbSet<HomeVideoSectionTranslation>? HomeVideoSectionTranslations { get; set; }
        public DbSet<Statistic>? Statistics { get; set; }
        public DbSet<StatisticTranslation>? StatisticTranslations { get; set; }
        public DbSet<ContactHeader>? ContactHeaders { get; set; }
        public DbSet<ContactHeaderTranslation>? ContactHeaderTranslations { get; set; }
        public DbSet<ContactInfo>? ContactInfos { get; set; }
        public DbSet<Information>? Informations { get; set; }
        public DbSet<Message>? Messages { get; set; }
        public DbSet<Footer>? Footers { get; set; }
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<AboutCounter>? AboutCounters { get; set; }
        public DbSet<AboutCounterTranslation>? AboutCounterTranslations { get; set; }
        public DbSet<AboutVideoSection>? AboutVideoSections { get; set; }
        public DbSet<AboutVideoSectionTranslation>? AboutVideoSectionTranslations { get; set; }
        public object Contacts { get; internal set; }
    }
}