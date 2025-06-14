namespace Pricing.Application.Events;

public record StatusEvent(string Host, string Application, Status Status) : IEvent
{
    public DateTime TimeStamp { get; init; } = DateTime.UtcNow;
    
    public string Key => $"status.{Application.ToLower()}";
}