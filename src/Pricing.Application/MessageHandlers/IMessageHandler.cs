using Pricing.Application.Messages;

namespace Pricing.Application.MessageHandlers;

public interface IMessageHandler
{
    Task HandleAsync(IMessage message);
}

public interface IMessageHandler<in TMessage> : IMessageHandler where TMessage : IMessage
{
    Task HandleAsync(TMessage message);
}