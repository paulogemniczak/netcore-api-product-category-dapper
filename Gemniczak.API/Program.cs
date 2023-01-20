using Gemniczak.API;
using Gemniczak.API.Config;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencyInjectionConfig();
builder.Services.AddApiVersioning(opt =>
{
	opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(4, 0);
	opt.AssumeDefaultVersionWhenUnspecified = true;
	opt.ReportApiVersions = true;
	opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
													new HeaderApiVersionReader("x-api-version"),
													new MediaTypeApiVersionReader("x-api-version"));
});
builder.Logging.AddConsole();
builder.Services.AddControllers(o =>
{
	o.Conventions.Add(new ActionHidingConvention());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig();
builder.Services.AddAuthenticationConfig(builder.Configuration);

var app = builder.Build();

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
	app.UseSwaggerConfig();
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
