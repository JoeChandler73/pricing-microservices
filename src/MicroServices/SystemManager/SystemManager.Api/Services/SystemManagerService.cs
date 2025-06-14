using Pricing.Application.Messaging;

namespace SystemManager.Api.Services;

public class SystemManagerService(
    IMessageConsumer _messageConsumer,
    IMessageBroker _messageBroker,
    ILogger<SystemManagerService> _logger) 
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting System Manager");
        
        _messageConsumer.StartAsync(_messageBroker, stoppingToken);
        
        return Task.CompletedTask;
    }
}