using RabbitMQ.Client;

namespace Pricing.Infrastructure.Messaging;

public interface IRabbitMqHelper : IDisposable
{
    Task<IChannel> GetChannelAsync();
}