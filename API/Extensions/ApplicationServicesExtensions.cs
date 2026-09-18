using API.Helpers;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Repositories;
using Infrastructure.Services;
using System.Reflection;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddScoped<IProductService, ProductService>(); 
            services.AddScoped<IProductRepository, ProductRepository>();

            var baseUrl = config["ExternalProductApi:BaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("'ExternalProductApi:BaseUrl' is not configured.");

            services.AddHttpClient<IExternalProductClient, ExternalProductClient>((sp, client) =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });
            services.AddAutoMapper(config =>
            {
                config.AddProfile<ProductMappingProfile>();
            });

            return services;
        }

    }
}
