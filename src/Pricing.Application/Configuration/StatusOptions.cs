namespace Pricing.Application.Configuration;

public record StatusOptions
{
    public required TimeSpan Interval { get; init; }
}