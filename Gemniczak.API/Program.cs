using Gemniczak.API;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
foreach (var type in Gemniczak.IoC.IoCConfiguration.GetDataTypes())
{
  builder.Services.AddTransient(type.Key, type.Value);
}

foreach (var type in Gemniczak.IoC.IoCConfiguration.GetAppServiceTypes())
{
  builder.Services.AddTransient(type.Key, type.Value);
}

foreach (var type in Gemniczak.API.IoC.Module.GetSingleTypes())
{
  builder.Services.AddTransient(type);
}

builder.Services.AddApiVersioning(opt =>
  {
      opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1,0);
      opt.AssumeDefaultVersionWhenUnspecified = true;
      opt.ReportApiVersions = true;
      opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                      new HeaderApiVersionReader("x-api-version"),
                                                      new MediaTypeApiVersionReader("x-api-version"));
  });

builder.Services.AddControllers(o =>
{
  o.Conventions.Add(new ActionHidingConvention());
});
builder.Services.AddEndpointsApiExplorer();

var filePath = Path.Combine(AppContext.BaseDirectory, "Gemniczak.xml");

builder.Services.AddSwaggerGen(options =>
{
  options.ExampleFilters();
  options.IncludeXmlComments(filePath, includeControllerXmlComments: true);
});
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetSection("Jwt:Audiance").Value
      };
    });
builder.Services.AddCors(option => option.AddPolicy(name: "NgOrigins",
    policy =>
    {
      policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }));

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
  app.UseSwagger();
  app.UseSwaggerUI(options =>
  {
    options.DocumentTitle = "Gemniczak API";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gemniczak");
    options.InjectJavascript("https://code.jquery.com/jquery-3.6.0.min.js");
    options.InjectJavascript("/swagger-script.js");
    options.InjectStylesheet("/theme-material.css");
  });
  app.UseHttpsRedirection();
}
else
{
  app.UseSwagger();
  app.UseSwaggerUI(options =>
  {
    options.DocumentTitle = "Gemniczak API";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gemniczak");
    options.InjectStylesheet("/theme-material.css");
    options.InjectJavascript("https://code.jquery.com/jquery-3.6.0.min.js");
    options.InjectJavascript("/swagger-script.js");
  });
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
