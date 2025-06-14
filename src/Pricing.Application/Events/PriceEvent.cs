namespace Pricing.Application.Events;

public record PriceEvent(string Symbol, decimal Price) : IEvent
{
    public DateTime TimeStamp = DateTime.UtcNow;
    
    public string Key => $"price.{Symbol.ToLower()}";
}