namespace Pricing.Application.Commands;

public record SubscribeCommand(string Symbol) : ICommand
{
    public string Key => $"subscribe.{Symbol.ToLower()}";
}