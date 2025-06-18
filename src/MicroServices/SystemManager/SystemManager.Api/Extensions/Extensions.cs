using Pricing.Infrastructure.Extensions;
using SystemManager.Api.MessageHandlers;
using SystemManager.Api.Services;

namespace SystemManager.Api.Extensions;

public static class Extensions
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.AddRabbitMessageBus();
        
        builder.Services
            .AddHostedService<SystemManagerService>()
            .AddSingleton<StatusEvenHandler>()
            .AddOpenApi()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();
        
        return builder;
    }
}