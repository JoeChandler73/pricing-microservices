using Pricing.Application.Events;
using Pricing.Application.MessageHandlers;

namespace PriceListener.Api.MessageHandlers;

public class PriceEventHandler(ILogger<PriceEventHandler> _logger) : MessageHandler<PriceEvent>
{
    public override Task HandleAsync(PriceEvent message)
    {
        _logger.LogInformation("Price event received: {0}", message);
        
        return Task.CompletedTask;
    }
}