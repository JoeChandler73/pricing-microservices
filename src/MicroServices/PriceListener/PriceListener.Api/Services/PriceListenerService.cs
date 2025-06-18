using PriceListener.Api.MessageHandlers;
using Pricing.Application.Events;
using Pricing.Application.Messaging;

namespace PriceListener.Api.Services;

public class PriceListenerService(
    IMessageBus messageBus,
    ILogger<PriceListenerService> _logger) 
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Price Listener");
        
        messageBus.Subscribe<PriceEvent, PriceEventHandler>();
        messageBus.StartAsync(stoppingToken);
        
        return Task.CompletedTask;
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("PriceListenerService is stopping.");

        await messageBus.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}