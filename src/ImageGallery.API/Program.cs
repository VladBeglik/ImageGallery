using System.Reflection;
using BookStore.API.Infrastructure;
using ImageGallery.API;
using ImageGallery.API.Infrastructure;
using ImageGallery.API.Infrastructure.Filters;
using ImageGallery.App.Infrastructure;
using ImageGallery.App.Infrastructure.Mapping;
using Microsoft.IdentityModel.Logging;
using NodaTime;
IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);
var appAssembly = typeof(AutoMapperProfile).GetTypeInfo().Assembly;
var pathBase = builder.Configuration["PATH_BASE"];

builder.Services
    .AddLocalStorage(builder.Configuration)
    .AddCustomIdentity(builder.Configuration, builder.Environment)
    .AddCustomSwagger(builder.Configuration)
    .AddAuthorization()
    .AddAutoMapper(appAssembly)
    .AddRouting(o => { o.LowercaseUrls = true; })
    .AddSingleton<IClock>(sp => SystemClock.Instance)
    .AddHttpContextAccessor()
    .AddCustomCors()
    .AddCustomValidation(appAssembly)
    .AddCustomMediatr(appAssembly);
    


builder.Services
    .AddControllers(o =>
    {

        o.Filters.Add<ModelBindingErrorFilter>();
        o.Filters.Add<OperationCancelledExceptionFilter>();
        o.Filters.Add<CustomExceptionFilter>();
    })
    .AddCustomJsonOptions(builder.Environment);


builder.Services
    .AddScoped<ICurrentUserService, CurrentUserService>()
    .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
    ;


var app = builder.Build();
app.Lifetime.ApplicationStarted.Register(app.DatabaseMigrate);

app.UseForwardedHeaders();

if (!string.IsNullOrEmpty(pathBase))
{
    app.UsePathBase(pathBase);
}


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint($"{pathBase}/swagger/v1/swagger.json", $"{nameof(ImageGallery)} API V1");
        //o.OAuthClientId("ro-1");

    }).UseCors(CorsPolicy.DEFAULT);
}


app.UseStaticFiles();
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.UseCors();

app
    .MapControllers()
    .RequireCors(CorsPolicy.DEFAULT);

app.Run();