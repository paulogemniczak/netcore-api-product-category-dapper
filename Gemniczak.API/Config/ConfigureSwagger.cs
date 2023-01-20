using System.Reflection;
using Swashbuckle.AspNetCore.Filters;

namespace Gemniczak.API.Config
{
    public static class ConfigureSwagger
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "Gemniczak.xml");

            services.AddSwaggerGen(options =>
            {
                options.ExampleFilters();
                options.IncludeXmlComments(filePath, includeControllerXmlComments: true);
            });
            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        }

        public static void UseSwaggerConfig(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./v1/swagger.json", "WebApi");
            });
        }
    }
}
