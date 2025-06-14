using Pricing.Application.Configuration;
using Pricing.Application.Messaging;
using Pricing.Application.Serialization;
using Pricing.Application.Services;
using Pricing.Infrastructure.Configuration;
using Pricing.Infrastructure.Messaging;
using Pricing.Infrastructure.Serialization;

namespace PriceManager.Api.Extensions;

public static class Extensions
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .Configure<StatusOptions>(builder.Configuration.GetSection("Status"))
            .Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"))
            .AddSingleton<IMessageSerializer, JsonMessageSerializer>()
            .AddSingleton<ITopicMapper, TopicMapper>()
            .AddSingleton<IMessageProducer, RabbitMqMessageProducer>()
            .AddHostedService<StatusService>()
            .AddOpenApi()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();
        
        return builder;
    }
}