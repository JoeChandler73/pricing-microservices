using Pricing.Application.MessageHandlers;
using Pricing.Application.Messages;

namespace Pricing.Application.Messaging;

public class MessageBus(
    IMessageProducer _messageProducer,
    IMessageConsumer _messageConsumer) : IMessageBus
{
    public Task Publish<TMessage>(TMessage message) where TMessage : IMessage
    {
        return _messageProducer.Publish(message);
    }

    public void Subscribe<TMessage, TMessageHandler>() where TMessage : IMessage where TMessageHandler : IMessageHandler<TMessage>
    {
        _messageConsumer.Subscribe<TMessage, TMessageHandler>();
    }

    public Task StartAsync(CancellationToken stoppingToken = default)
    {
        return _messageConsumer.StartAsync(stoppingToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _messageProducer.DisposeAsync();
        await _messageConsumer.DisposeAsync();
    }
}