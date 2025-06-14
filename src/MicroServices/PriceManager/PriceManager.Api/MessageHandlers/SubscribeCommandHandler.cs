using Pricing.Application.Commands;
using Pricing.Application.MessageHandlers;
using Pricing.Application.Messages;

namespace PriceManager.Api.MessageHandlers;

public class SubscribeCommandHandler(ILogger<SubscribeCommandHandler> _logger) 
    : IMessageHandler<SubscribeCommand>
{
    public Task HandleAsync(SubscribeCommand message)
    {
        _logger.LogInformation("Subscribe command received: {0}", message);
        
        return Task.CompletedTask;
    }

    public Task HandleAsync(IMessage message)
    {
        return HandleAsync(message as SubscribeCommand);
    }
}