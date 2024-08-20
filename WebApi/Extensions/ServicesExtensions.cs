
using AspNetCoreRateLimit;
using Entities.DataTransferObject;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
using Presentation.Controllers;
using Repositories.Contracts;
using Repositories.EfCore;
using Services;
using Services.Contracts;

namespace WebApi.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

        }

        public static void ConfigureServiceManager(this IServiceCollection services) => services.AddScoped<IServiceManager, ServiceManager>();
        public static void ConfigureRepositoryManager(this IServiceCollection services) => services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureLoggerService(this IServiceCollection services) => services.AddSingleton<ILogerService, LoggerManager>();
        public static void ConfigureActionFilter(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddSingleton<LogFilterAttribute>();
            services.AddScoped<ValidateMediaTypeAttribute>();
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithHeaders("X-Pagination");
                });
            });
        }
        public static void ConfigureDataShaper(this IServiceCollection services)
        {
            services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();
        }
        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                var systemTextJsonOutputFormatter = options.OutputFormatters
                    .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();
                if (systemTextJsonOutputFormatter is not null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.btkakademi.hateoas+json");

                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.btkakademi.apiroot+json");
                }
                var xmlOutputFormatter = options
                .OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();
                if (xmlOutputFormatter is not null)
                {
                    xmlOutputFormatter.SupportedMediaTypes
                        .Add("application/vnd.btkakademi.hateoas+xml");
                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.btkakademi.apiroot+xml");
                }
            });
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning
                (options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                    options.Conventions.Controller<BooksController>()
                    .HasApiVersion(new ApiVersion(1, 0));
                    options.Conventions.Controller<BooksV2Controller>()
                    .HasDeprecatedApiVersion(new ApiVersion(2, 0));

                });
        }
        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }
        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        {
            services.AddHttpCacheHeaders(expireOptions =>
            {
                expireOptions.MaxAge = 70;
                expireOptions.CacheLocation = CacheLocation.Private;
            },

            validationOpt =>
            {
                validationOpt.MustRevalidate= false;
            }


            ); 
        }
        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>()
            {
                new RateLimitRule()
                {
                    Endpoint="*",
                    Limit=10,
                    Period="1m"
                }
            };

            services.Configure<IpRateLimitOptions>(op =>
            {
                op.GeneralRules = rateLimitRules;
            });

            services.AddSingleton<IRateLimitCounterStore,MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        }
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
            
        }
    }
}
