
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
using Repositories.Contracts;
using Repositories.EfCore;
using Services;
using Services.Contracts;

namespace WebApi.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

        }

        public static void ConfigureServiceManager(this IServiceCollection services) => services.AddScoped<IServiceManager, ServiceManager>();
        public static void ConfigureRepositoryManager(this IServiceCollection services)=>services.AddScoped<IRepositoryManager,RepositoryManager>();

        public static void ConfigureLoggerService(this IServiceCollection services)=>services.AddSingleton<ILogerService,LoggerManager>();
        public static void ConfigureActionFilter(this IServiceCollection services){
            services.AddScoped<ValidationFilterAttribute>();
            services.AddSingleton<LogFilterAttribute>();
        }
    }
}
