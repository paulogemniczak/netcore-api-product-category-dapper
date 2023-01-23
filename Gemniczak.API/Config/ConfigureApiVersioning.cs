using Microsoft.AspNetCore.Mvc.Versioning;

namespace Gemniczak.API.Config
{
	public static class ConfigureApiVersioning
	{
		public static void AddApiVersioningConfig(this IServiceCollection services)
		{
			services.AddApiVersioning(opt =>
			{
				opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
				opt.AssumeDefaultVersionWhenUnspecified = true;
				opt.ReportApiVersions = true;
				opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
																new HeaderApiVersionReader("x-api-version"),
																new MediaTypeApiVersionReader("x-api-version"));
			});
			services.AddVersionedApiExplorer(setup =>
			{
				setup.GroupNameFormat = "'v'VVV";
				setup.SubstituteApiVersionInUrl = true;
			});
		}
	}
}
