using BE_CourseSelling.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using BE_CourseSelling.Core.Interfaces.Repositories;
using BE_CourseSelling.Infrastructure.Repositories;
using BE_CourseSelling.Core.Interfaces.Services;
using BE_CourseSelling.Service.Services;

namespace BE_CourseSelling.Api.Configurations
{
    public static class ServiceConfigs
    {
        public static IServiceCollection AddServiceConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly("BE_CourseSelling.Infrastructure")
                )
            );

            // Register repository and service
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICourseService, CourseService>();

            // AutoMapper configuration: scan mapping profiles in the Service assembly
            services.AddAutoMapper(typeof(BE_CourseSelling.Service.Mappings.CourseMappingProfile).Assembly);

            return services;
        }
    }
}
