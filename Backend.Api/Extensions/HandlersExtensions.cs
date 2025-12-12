using Backend.Application.Commands;
namespace Backend.Api.Extensions; 

public static class HandlerExtensions
{
    public static void  AddApplicationHandlers (this IServiceCollection services)
    {
        services.AddScoped<ICreateOrderCommandHandler, CreateOrderCommandHandler>();
        services.AddScoped<IEditOrderCommandHandler, EditOrderCommandHandler>();
        services.AddScoped<IDeleteOrderCommandHandler, DeleteOrderCommandHandler>();
    }
}