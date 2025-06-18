using Pricing.Application.Messages;

namespace Pricing.Application.Messaging;

public interface IMessageProducer : IAsyncDisposable
{
    Task Publish<TMessage>(TMessage message) where TMessage : IMessage;
}