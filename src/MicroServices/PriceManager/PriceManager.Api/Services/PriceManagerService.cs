using Pricing.Application.Messaging;

namespace PriceManager.Api.Services;

public class PriceManagerService(
    IMessageConsumer _messageConsumer,
    IMessageBroker _messageBroker,
    ILogger<PriceManagerService> _logger) 
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Price Manager.");
        
        _messageConsumer.StartAsync(_messageBroker, stoppingToken);
        
        return Task.CompletedTask;
    }
}