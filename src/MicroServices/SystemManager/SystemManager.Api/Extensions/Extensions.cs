using Pricing.Application.Messaging;
using Pricing.Application.Serialization;
using Pricing.Infrastructure.Configuration;
using Pricing.Infrastructure.Messaging;
using Pricing.Infrastructure.Serialization;
using SystemManager.Api.MessageHandlers;
using SystemManager.Api.Services;

namespace SystemManager.Api.Extensions;

public static class Extensions
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"))
            .AddSingleton<IMessageSerializer, JsonMessageSerializer>()
            .AddSingleton<IMessageProducer, RabbitMqMessageProducer>()
            .AddSingleton<StatusEvenHandler>()
            .AddSingleton<IMessageConsumer, RabbitMqMessageConsumer>()
            .AddHostedService<SystemManagerService>()
            .AddOpenApi()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();
        
        return builder;
    }
}