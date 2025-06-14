using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pricing.Application.MessageHandlers;
using Pricing.Application.Messaging;
using Pricing.Application.Serialization;
using Pricing.Infrastructure.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Pricing.Infrastructure.Messaging;

public class RabbitMqMessageConsumer(
    IMessageSerializer _messageSerializer,
    IOptions<RabbitMqOptions> _options,
    ILogger<RabbitMqMessageConsumer> _logger) 
    : RabbitMqBase(_options), IMessageConsumer
{
    public async Task StartAsync(IMessageHandler messageHandler, CancellationToken stoppingToken = default)
    {
        var channel = await GetChannelAsync();
        
        await channel.QueueDeclareAsync(_options.Value.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        
        foreach (var routingKey in _options.Value.RoutingKeys.Split(','))
            await channel.QueueBindAsync(queue: _options.Value.QueueName, exchange: _options.Value.ExchangeName, routingKey: routingKey);
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, args) =>
        {
            var messageType = Type.GetType(args.BasicProperties.Type!);
            var message = _messageSerializer.Deserialize(args.Body.ToArray(), messageType!);
            
            try
            {
                await messageHandler.HandleAsync(message);
                await channel.BasicAckAsync(args.DeliveryTag, false);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Something went wrong with Consumer_Received!");
            }
        };
        
        var consumerTag = 
            await channel.BasicConsumeAsync(_options.Value.QueueName, false, consumer, cancellationToken: stoppingToken);
    }
}