using Pricing.Application.Events;
using Pricing.Application.Messaging;
using SystemManager.Api.MessageHandlers;

namespace SystemManager.Api.Services;

public class SystemManagerService(
    IMessageConsumer _messageConsumer,
    ILogger<SystemManagerService> _logger) 
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting System Manager");
        
        _messageConsumer.Subscribe<StatusEvent, StatusEvenHandler>();
        _messageConsumer.StartAsync(stoppingToken);
        
        return Task.CompletedTask;
    }
}