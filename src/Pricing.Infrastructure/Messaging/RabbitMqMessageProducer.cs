using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pricing.Application.Messages;
using Pricing.Application.Messaging;
using Pricing.Application.Serialization;
using Pricing.Infrastructure.Configuration;
using RabbitMQ.Client;

namespace Pricing.Infrastructure.Messaging;

public class RabbitMqMessageProducer(
    IMessageSerializer _messageSerializer,
    IOptions<RabbitMqOptions> _options,
    ILogger<RabbitMqMessageProducer> _logger) 
    : RabbitMqBase(_options), IMessageProducer
{
    private IConnection? _connection;
    private IChannel? _channel;
    
    public async Task Publish<TMessage>(TMessage message) where TMessage : IMessage
    {
        var channel = await GetChannelAsync();
        
        var body = _messageSerializer.Serialize(message);
        
        var properties = new BasicProperties
        {
            ContentType = "application/json",
            Type = typeof(TMessage).AssemblyQualifiedName
        };

        await channel.BasicPublishAsync(
            _options.Value.ExchangeName, message.Key, true, properties, body);
        
        _logger.LogInformation("Message sent: {0}", message);
    }
}