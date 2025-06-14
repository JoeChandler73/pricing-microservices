namespace Pricing.Application.Messaging;

public interface ITopicMapper
{
    Type MapTopicToType(string topic);
}