using Microsoft.Extensions.Options;
using Pricing.Infrastructure.Configuration;
using RabbitMQ.Client;

namespace Pricing.Infrastructure.Messaging;

public abstract class RabbitMqBase(IOptions<RabbitMqOptions> _options) : IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    
    protected async Task<IChannel> GetChannelAsync()
    {
        if (!(_connection?.IsOpen ?? false))
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_options.Value.Uri),
                ClientProvidedName = _options.Value.ClientName
            };
        
            _connection = await connectionFactory.CreateConnectionAsync();
        }
        
        if (!(_channel?.IsOpen ?? false))
        {
            _channel = await _connection!.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(_options.Value.ExchangeName, ExchangeType.Topic);
        }
        
        return _channel!;
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }

        if (_channel != null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }
    }
}