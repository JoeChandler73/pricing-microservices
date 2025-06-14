using Pricing.Application.Messaging;

namespace PriceListener.Api.Services;

public class PriceListenerService(
    IMessageConsumer _messageConsumer,
    IMessageBroker _messageBroker,
    ILogger<PriceListenerService> _logger) 
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Price Listener");
        
        _messageConsumer.StartAsync(_messageBroker, stoppingToken);
        
        return Task.CompletedTask;
    }
}