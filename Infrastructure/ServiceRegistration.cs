using Application.CustomAttributes.Services;
using Application.Interfaces.MailService;
using Application.JwtTokenHandlerInterface;
using Application.StorageInterfaces;
using Application.StorageInterfaces.Cloudinary;
using ECommerce.Infrastructure.MailServiceConcrete;
using ECommerce.Infrastructure.Pipelines.Logging;
using Infrastructure.CustomAttributeConfigurations;
using Infrastructure.JwtTokenHandlerConcretes;
using Infrastructure.Logging.Serilog;
using Infrastructure.Logging.Serilog.Loggers;
using Infrastructure.Pipelines.Caching;
using Infrastructure.Pipelines.Transaction;
using Infrastructure.Pipelines.Validation;
using Infrastructure.StorageConcretes;
using Infrastructure.StorageConcretes.CloudinaryStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
            configuration.AddOpenBehavior(typeof(CacheRemovingBehavior<,>));


        });
        services.AddSingleton<LoggerServiceBase, FileLogger>();

        services.AddScoped<IStorageService, StorageService>();

        services.AddScoped<ITokenHandler, TokenHandler>();

       

        services.AddScoped<IMailService, MailService>();


        services.AddSingleton<ICloudinaryStorage>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new CloudinaryStorage(configuration);
        });

        services.AddScoped<IApplicationService, ApplicationService>();



        return services;
    }

    public static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage
    {
        services.AddScoped<IStorage, T>();
    }

}