using PriceManager.Api.MessageHandlers;
using Pricing.Application.Commands;
using Pricing.Application.Messaging;

namespace PriceManager.Api.Services;

public class PriceManagerService(
    IMessageBus messageBus,
    ILogger<PriceManagerService> _logger) 
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Price Manager.");
        
        messageBus.Subscribe<SubscribeCommand, SubscribeCommandHandler>();
        messageBus.StartAsync(stoppingToken);
        
        return Task.CompletedTask;
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("PriceManagerService is stopping.");

        await messageBus.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}