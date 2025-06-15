using PriceListener.Api.MessageHandlers;
using Pricing.Application.Events;
using Pricing.Application.Messaging;

namespace PriceListener.Api.Services;

public class PriceListenerService(
    IMessageConsumer _messageConsumer,
    ILogger<PriceListenerService> _logger) 
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Price Listener");
        
        _messageConsumer.Subscribe<PriceEvent, PriceEventHandler>();
        _messageConsumer.StartAsync(stoppingToken);
        
        return Task.CompletedTask;
    }
}