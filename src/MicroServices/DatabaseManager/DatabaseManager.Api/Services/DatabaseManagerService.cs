using Pricing.Application.Messaging;

namespace DatabaseManager.Api.Services;

public class DatabaseManagerService(
    IMessageConsumer _messageConsumer,
    IMessageBroker _messageBroker,
    ILogger<DatabaseManagerService> _logger) 
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Database Manager");
        
        _messageConsumer.StartAsync(_messageBroker, stoppingToken);
        
        return Task.CompletedTask;
    }
}