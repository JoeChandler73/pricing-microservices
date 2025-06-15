using DatabaseManager.Api.MessageHandlers;
using Pricing.Application.Events;
using Pricing.Application.Messaging;

namespace DatabaseManager.Api.Services;

public class DatabaseManagerService(
    IMessageConsumer _messageConsumer,
    ILogger<DatabaseManagerService> _logger) 
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Database Manager");
        
        _messageConsumer.Subscribe<PriceEvent, PriceEventHandler>();
        _messageConsumer.StartAsync(stoppingToken);
        
        return Task.CompletedTask;
    }
}