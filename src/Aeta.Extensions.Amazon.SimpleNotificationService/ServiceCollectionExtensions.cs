using System;
using Amazon.SimpleNotificationService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aeta.Extensions.Amazon.SimpleNotificationService
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAmazonSns(this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped,
            Action<AmazonSimpleNotificationServiceConfig> configure = null)
        {
            var serviceDescriptor = new ServiceDescriptor(typeof(IAmazonSimpleNotificationService), serviceProvider =>
            {
                var config = serviceProvider.GetAmazonSnsConfig();
                configure?.Invoke(config);

                return new AmazonSimpleNotificationServiceClient(config);
            }, lifetime);

            services.Add(serviceDescriptor);
            return services;
        }
        
        private static AmazonSimpleNotificationServiceConfig GetAmazonSnsConfig(this IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService<IConfiguration>()?
                .GetSection("Amazon")?
                .GetSection("Sns");

            var amazonConfig = new AmazonSimpleNotificationServiceConfig();
            if (configuration is null) return amazonConfig;

            configuration.Bind(amazonConfig);
            return amazonConfig;
        }
    }
}