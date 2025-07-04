using Pricing.Application.MessageHandlers;
using Pricing.Application.Messages;

namespace Pricing.Application.Messaging;

public interface IMessageConsumer: IAsyncDisposable
{
    void Subscribe<TMessage, TMessageHandler>()
        where TMessage : IMessage where TMessageHandler : IMessageHandler<TMessage>;
    
    Task StartAsync(CancellationToken stoppingToken = default);
}