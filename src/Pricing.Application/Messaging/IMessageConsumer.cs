using Pricing.Application.MessageHandlers;

namespace Pricing.Application.Messaging;

public interface IMessageConsumer
{
    Task StartAsync(IMessageHandler messageHandler, CancellationToken stoppingToken = default);
}