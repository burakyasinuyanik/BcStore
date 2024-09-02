using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NLog;
using Presentation.ActionFilters;
using Repositories.EfCore;
using Services.Contracts;
using WebApi.Extensions;


var builder = WebApplication.CreateBuilder(args);
//xml log kayýt satýrý
LogManager.Setup().LoadConfigurationFromFile(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.

//içerik pazarlýðý
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
    config.CacheProfiles.Add("5mins", new CacheProfile() { Duration = 300 });
})
    .AddXmlDataContractSerializerFormatters()
    .AddCustomCsvFormatter()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
   .AddNewtonsoftJson(opt =>
   {
       opt.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore;
   });


//builder.Services.AddScoped<ValidationFilterAttribute>();// ýoc her kiþi için özel oluþturma


//406-422 kodlarý açmak için kullanýlmaktadýr.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{ options.SuppressModelStateInvalidFilter = true; });

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureActionFilter();
builder.Services.ConfigureCors();
builder.Services.ConfigureDataShaper();
builder.Services.AddCustomMediaTypes();
builder.Services.AddScoped<IBookLinks, BookLinks>();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();

//builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogerService>();
app.ConfigureExceptionHandler(logger);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "Btk Akedemi v1");
        s.SwaggerEndpoint("/swagger/v2/swagger.json", "Btk Akedemi v2");
    });
}
if (app.Environment.IsProduction())
{
    app.UseHsts();
}
app.UseIpRateLimiting();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseResponseCaching();
app.UseHttpCacheHeaders();

//giriþ iþlemleri
app.UseAuthentication();
//yetkilendirme
app.UseAuthorization();

app.MapControllers();

app.Run();
