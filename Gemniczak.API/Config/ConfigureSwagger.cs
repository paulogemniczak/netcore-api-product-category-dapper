using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Gemniczak.API.Config
{
    public static class ConfigureSwagger
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            services.AddSwaggerGen(options =>
            {
                // options.ExampleFilters();
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });
            // services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        }

        public static void UseSwaggerConfig(this WebApplication app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

			var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // options.SwaggerEndpoint("./v1/swagger.json", "Gemniczak.API");
                // options.SwaggerEndpoint("./v2/swagger.json", "Gemniczak.API");
				foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
				{
					options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
				}
            });
        }
    }
}
