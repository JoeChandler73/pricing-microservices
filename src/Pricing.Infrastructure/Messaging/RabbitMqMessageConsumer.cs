using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pricing.Application.MessageHandlers;
using Pricing.Application.Messages;
using Pricing.Application.Messaging;
using Pricing.Application.Serialization;
using Pricing.Infrastructure.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Pricing.Infrastructure.Messaging;

public class RabbitMqMessageConsumer(
    IChannelFactory _channelFactory,
    IMessageSerializer _messageSerializer,
    IServiceScopeFactory _serviceScopeFactory,
    IOptions<RabbitMqOptions> _options,
    ILogger<RabbitMqMessageConsumer> _logger) 
    : IMessageConsumer
{
    private readonly List<Type> _messageTypes = new();
    private readonly Dictionary<string, List<Type>> _handlerTypes = new();
    
    public void Subscribe<TMessage, TMessageHandler>()
        where TMessage : IMessage where TMessageHandler : IMessageHandler<TMessage>
    {
        var messageTypeName = typeof(TMessage).AssemblyQualifiedName!;
        var handlerType = typeof(TMessageHandler);
        
        if (!_messageTypes.Contains(typeof(TMessage)))
            _messageTypes.Add(typeof(TMessage));

        if (!_handlerTypes.ContainsKey(messageTypeName))
            _handlerTypes.Add(messageTypeName, []);
        
        _handlerTypes[messageTypeName].Add(handlerType);
    }
    
    public async Task StartAsync(CancellationToken stoppingToken = default)
    {
        var channel = await _channelFactory.GetChannelAsync();
        
        await channel.QueueDeclareAsync(
            _options.Value.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        
        foreach (var messageType in _messageTypes)
        {
            await channel.QueueBindAsync(
                queue: _options.Value.QueueName, exchange: _options.Value.ExchangeName, routingKey: messageType.Name);
        }

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += MessageReceived;
        
        await channel.BasicConsumeAsync(
            _options.Value.QueueName, false, consumer, cancellationToken: stoppingToken);
    }
    
    private async Task MessageReceived(object sender, BasicDeliverEventArgs args)
    {
        var messageTypeName = args.BasicProperties.Type!;

        try
        {
            await ProcessMessage(messageTypeName, args.Body.ToArray()).ConfigureAwait(false);
            await ((AsyncDefaultBasicConsumer)sender).Channel.BasicAckAsync(args.DeliveryTag, multiple: false);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong with MessageReceived!");
        }
    }
    
    private async Task ProcessMessage(string messageTypeName, byte[] messageBytes)
    {
        if (_handlerTypes.TryGetValue(messageTypeName, out var handlerTypes))
        {
            using var scope = _serviceScopeFactory.CreateScope();
            foreach (var handlerType in handlerTypes)
            {
                var handler = scope.ServiceProvider.GetService(handlerType);
                        
                if(handler == null)
                    continue;
                        
                var messageType = _messageTypes.SingleOrDefault(t => t.AssemblyQualifiedName == messageTypeName);
                var @message = _messageSerializer.Deserialize(messageBytes, messageType!);
                var concreteType = typeof(IMessageHandler<>).MakeGenericType(messageType!);

                await (Task)concreteType.GetMethod("HandleAsync").Invoke(handler, [message]);
            }
        }
    }
    
    public async ValueTask DisposeAsync()
    {
        await _channelFactory.DisposeAsync();
    }
}