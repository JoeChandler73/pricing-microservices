using Pricing.Application.MessageHandlers;
using Pricing.Application.Messages;

namespace Pricing.Application.Messaging;

public interface IMessageBroker : IMessageHandler, IDisposable
{
    public Task Publish<T>(T message) where T : IMessage;
    
    public Task Subscribe<T>(IMessageHandler<T> handler) where T : IMessage;
    
    public Task Subscribe<T>(Func<T, Task> handler) where T : IMessage;
}