using Pricing.Application.Messages;

namespace Pricing.Application.MessageHandlers;

public abstract class MessageHandler<TMessage> : IMessageHandler<TMessage> where TMessage : IMessage
{
    public abstract Task HandleAsync(TMessage message);
    
    public Task HandleAsync(IMessage message)
    {
        return HandleAsync((TMessage)message);
    }
}