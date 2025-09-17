using Application.AuthorizeAndAuthentication.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Application.Interfaces.RepositoryServices;
using Application.Interfaces.RepositoryServices.EndpointRepositories;
using Application.Interfaces.RepositoryServices.MenuRepositories;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Authorize;
using Persistence.Contexts;
using Persistence.Repositories;
using Persistence.Repositories.EndpointRepository;
using Persistence.Repositories.MenuRepository;


namespace Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BaseDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgreSql")));


        services.AddScoped<IMenuWriteRepository, MenuWriteRepository>();
        services.AddScoped<IMenuReadRepository, MenuReadRepository>();

        services.AddScoped<IEndpointWriteRepository, EndpointWriteRepository>();
        services.AddScoped<IEndpointReadRepository, EndpointReadRepository>();

        services.AddScoped<IDeviceReadRepository, DeviceReadRepository>();
        services.AddScoped<IDeviceWriteRepository, DeviceWriteRepository>();
        services.AddScoped<IGatewayReadRepository, GatewayReadRepository>();
        services.AddScoped<IGatewayWriteRepository, GatewayWriteRepository>();
        services.AddScoped<IEnergyDataReadRepository, EnergyDataReadRepository>();
        services.AddScoped<IEnergyDataWriteRepository, EnergyDataWriteRepository>();


        services.AddScoped<IAuthorizationEndpointService, AuthorizationEndpointService>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<IUserIdService, UserIdService>();
    }
}
