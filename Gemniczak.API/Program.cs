using Gemniczak.API;
using Gemniczak.API.Config;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencyInjectionConfig();
builder.Services.AddControllers(o =>
{
    o.Conventions.Add(new ActionHidingConvention());
});
builder.Services.AddApiVersioningConfig();
builder.Logging.AddConsole();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerConfig();
builder.Services.AddAuthenticationConfig(builder.Configuration);
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

WebApplication app = builder.Build();

app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next(context);
});

app.UseForwardedHeaders();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.Run();
}
else
{
#if PRODUCAO
  app.Run("http://localhost:5000");
#elif HOMOLOGACAO
  app.Run("http://localhost:5001");
#else
    app.Run("http://localhost:5002");
#endif
}
