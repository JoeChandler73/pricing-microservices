using DatabaseManager.Api.MessageHandlers;
using DatabaseManager.Api.Services;
using Pricing.Application.Configuration;
using Pricing.Application.Events;
using Pricing.Application.MessageHandlers;
using Pricing.Application.Messaging;
using Pricing.Application.Serialization;
using Pricing.Application.Services;
using Pricing.Infrastructure.Configuration;
using Pricing.Infrastructure.Messaging;
using Pricing.Infrastructure.Serialization;

namespace DatabaseManager.Api.Extensions;

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
            .AddSingleton<IMessageHandler<PriceEvent>, PriceEventHandler>()
            .AddSingleton<IMessageBroker>(provider =>
            {
                var messageBroker = new RxMessageBroker();
                messageBroker.Subscribe(provider.GetRequiredService<IMessageHandler<PriceEvent>>());
                return messageBroker;
            })
            .AddSingleton<IMessageConsumer, RabbitMqMessageConsumer>()
            .AddHostedService<DatabaseManagerService>()
            .AddHostedService<StatusService>()
            .AddOpenApi()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();
        
        return builder;
    }
}