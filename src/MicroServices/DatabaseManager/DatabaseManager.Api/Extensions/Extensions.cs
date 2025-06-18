using DatabaseManager.Api.MessageHandlers;
using DatabaseManager.Api.Services;
using Pricing.Application.Configuration;
using Pricing.Infrastructure.Extensions;

namespace DatabaseManager.Api.Extensions;

public static class Extensions
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.AddRabbitMessageBus();
        builder.AddStatusService();
                    
        builder.Services
            .Configure<StatusOptions>(builder.Configuration.GetSection("Status"))
            .AddHostedService<DatabaseManagerService>()
            .AddSingleton<PriceEventHandler>()
            .AddOpenApi()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();
        
        return builder;
    }
}