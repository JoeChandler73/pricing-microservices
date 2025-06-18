using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pricing.Application.Configuration;
using Pricing.Application.Messaging;
using Pricing.Application.Serialization;
using Pricing.Application.Services;
using Pricing.Infrastructure.Configuration;
using Pricing.Infrastructure.Messaging;
using Pricing.Infrastructure.Serialization;

namespace Pricing.Infrastructure.Extensions;

public static class Extensions
{
    public static WebApplicationBuilder AddRabbitMessageBus(this WebApplicationBuilder builder)
    {
        builder.Services
            .Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"))
            .AddSingleton<IMessageSerializer, JsonMessageSerializer>()
            .AddSingleton<IChannelFactory, RabbitMqChannelFactory>()
            .AddSingleton<IMessageProducer, RabbitMqMessageProducer>()
            .AddSingleton<IMessageConsumer, RabbitMqMessageConsumer>()
            .AddSingleton<IMessageBus, MessageBus>();
        
        return builder;
    }
    
    public static WebApplicationBuilder AddStatusService(this WebApplicationBuilder builder)
    {
        builder.Services
            .Configure<StatusOptions>(builder.Configuration.GetSection("Status"))
            .AddHostedService<StatusService>();
        
        return builder;
    }
}