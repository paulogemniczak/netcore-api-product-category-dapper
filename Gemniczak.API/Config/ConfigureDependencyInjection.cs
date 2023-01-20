namespace Gemniczak.API.Config
{
    public static class ConfigureDependencyInjection
    {
        public static IServiceCollection AddDependencyInjectionConfig(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            foreach (var type in Gemniczak.IoC.IoCConfiguration.GetDataTypes())
            {
                services.AddTransient(type.Key, type.Value);
            }

            foreach (var type in Gemniczak.IoC.IoCConfiguration.GetAppServiceTypes())
            {
                services.AddTransient(type.Key, type.Value);
            }

            foreach (var type in Gemniczak.API.IoC.Module.GetSingleTypes())
            {
                services.AddTransient(type);
            }

            return services;
        }
    }

}
