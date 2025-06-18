using RabbitMQ.Client;

namespace Pricing.Infrastructure.Messaging;

public interface IChannelFactory : IAsyncDisposable
{
    Task<IChannel> GetChannelAsync();
}