using Pricing.Application.Events;
using Pricing.Application.Messaging;
using SystemManager.Api.MessageHandlers;

namespace SystemManager.Api.Services;

public class SystemManagerService(
    IMessageBus messageBus,
    ILogger<SystemManagerService> _logger) 
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting System Manager");
        
        messageBus.Subscribe<StatusEvent, StatusEvenHandler>();
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