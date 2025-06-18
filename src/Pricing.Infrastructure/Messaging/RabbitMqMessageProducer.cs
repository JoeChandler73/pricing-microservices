using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pricing.Application.Messages;
using Pricing.Application.Messaging;
using Pricing.Application.Serialization;
using Pricing.Infrastructure.Configuration;
using RabbitMQ.Client;

namespace Pricing.Infrastructure.Messaging;

public class RabbitMqMessageProducer(
    IChannelFactory _channelFactory,
    IMessageSerializer _messageSerializer,
    IOptions<RabbitMqOptions> _options,
    ILogger<RabbitMqMessageProducer> _logger) 
    : IMessageProducer
{
    private IConnection? _connection;
    private IChannel? _channel;
    
    public async Task Publish<TMessage>(TMessage message) where TMessage : IMessage
    {
        var channel = await _channelFactory.GetChannelAsync();

        var body = _messageSerializer.Serialize(message);
        
        var properties = new BasicProperties
        {
            ContentType = "application/json",
            Type = typeof(TMessage).AssemblyQualifiedName
        };
        
        await channel.BasicPublishAsync(
            _options.Value.ExchangeName, typeof(TMessage).Name, true, properties, body);
        
        _logger.LogInformation("Message sent: {0}", message);
    }
    
    public async ValueTask DisposeAsync()
    {
        await _channelFactory.DisposeAsync();
    }
}