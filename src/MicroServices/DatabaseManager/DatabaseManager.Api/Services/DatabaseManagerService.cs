using DatabaseManager.Api.MessageHandlers;
using Pricing.Application.Events;
using Pricing.Application.Messaging;

namespace DatabaseManager.Api.Services;

public class DatabaseManagerService(
    IMessageBus messageBus,
    ILogger<DatabaseManagerService> _logger) 
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Database Manager");
        
        messageBus.Subscribe<PriceEvent, PriceEventHandler>();
        messageBus.StartAsync(stoppingToken);
        
        return Task.CompletedTask;
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("DatabaseManagerService is stopping.");

        await messageBus.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}