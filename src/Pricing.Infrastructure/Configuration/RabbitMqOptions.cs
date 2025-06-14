namespace Pricing.Infrastructure.Configuration;

public record RabbitMqOptions
{
    public required string Uri { get; init; }
    
    public required string ClientName { get; init; }
    
    public required string ExchangeName { get; init; }
    
    public string? QueueName { get; init; }
    
    public string? RoutingKeys { get; init; }
}
