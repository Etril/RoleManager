using Backend.Api.Services.Auth;
using Backend.Api.Services.Interfaces;
using Backend.Application.Repositories;
using Backend.Infrastructure.Persistence.Repositories;

namespace Backend.Api.Extensions; 

public static class ServiceExtensions
{
    public static void AddApplicationServices (this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IAuthService, AuthService>();
    }
}