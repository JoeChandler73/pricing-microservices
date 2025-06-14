using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pricing.Infrastructure.Configuration;
using RabbitMQ.Client;

namespace Pricing.Infrastructure.Messaging;

public class RabbitMqHelper(
    IOptions<RabbitMqOptions> _options,
    ILogger<RabbitMqHelper> _logger) : IRabbitMqHelper
{
    private IConnection? _connection;
    private IChannel? _channel;
    
    public async Task<IChannel> GetChannelAsync()
    {
        if (!_connection?.IsOpen ?? false)
        {
            _logger.LogInformation("Creating new RabbitMq connection.");
            
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_options.Value.Uri),
                ClientProvidedName = _options.Value.ClientName
            };
        
            _connection = await connectionFactory.CreateConnectionAsync();
        }
        
        if (!_channel?.IsOpen ?? false)
        {
            _logger.LogInformation("Creating new RabbitMq channel.");
            
            _channel = await _connection!.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(_options.Value.ExchangeName, ExchangeType.Topic);
        }
        
        return _channel!;
    }
    
    public void Dispose()
    {
        _connection?.Dispose();
        _channel?.Dispose();
    }
}