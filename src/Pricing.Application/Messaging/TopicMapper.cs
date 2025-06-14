using Pricing.Application.Commands;
using Pricing.Application.Events;

namespace Pricing.Application.Messaging;

public class TopicMapper : ITopicMapper
{
    public Type MapTopicToType(string topic)
    {
        if (topic.StartsWith("status"))
            return typeof(StatusEvent);
        else if (topic.StartsWith("subscribe"))
            return typeof(SubscribeCommand);
        else
            throw new NotSupportedException($"Could not map topic {topic} to a message type");
    }
}