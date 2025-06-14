using Pricing.Application.Events;
using Pricing.Application.MessageHandlers;

namespace SystemManager.Api.MessageHandlers;

public class StatusEvenHandler(ILogger<StatusEvenHandler> _logger) : MessageHandler<StatusEvent>
{
    public override Task HandleAsync(StatusEvent message)
    {
        _logger.LogInformation("Status event received: {0}", message);
        
        return Task.CompletedTask;
    }
}