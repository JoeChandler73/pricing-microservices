using PriceManager.Api.MessageHandlers;
using Pricing.Application.Commands;
using Pricing.Application.Messaging;

namespace PriceManager.Api.Services;

public class PriceManagerService(
    IMessageConsumer _messageConsumer,
    ILogger<PriceManagerService> _logger) 
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Price Manager.");
        
        _messageConsumer.Subscribe<SubscribeCommand, SubscribeCommandHandler>();
        _messageConsumer.StartAsync(stoppingToken);
        
        return Task.CompletedTask;
    }
}