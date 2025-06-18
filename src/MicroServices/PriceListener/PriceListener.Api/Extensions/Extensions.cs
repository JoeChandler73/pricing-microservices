using PriceListener.Api.MessageHandlers;
using PriceListener.Api.Services;
using Pricing.Application.Configuration;
using Pricing.Infrastructure.Extensions;

namespace PriceListener.Api.Extensions;

public static class Extensions
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.AddRabbitMessageBus();
        builder.AddStatusService();
        
        builder.Services
            .Configure<StatusOptions>(builder.Configuration.GetSection("Status"))
            .AddHostedService<PriceListenerService>()
            .AddSingleton<PriceEventHandler>()
            .AddOpenApi()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();
        
        return builder;
    }
}