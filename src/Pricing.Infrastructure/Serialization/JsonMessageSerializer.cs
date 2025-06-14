using System.Text;
using Newtonsoft.Json;
using Pricing.Application.Messages;
using Pricing.Application.Serialization;

namespace Pricing.Infrastructure.Serialization;

public class JsonMessageSerializer : IMessageSerializer
{
    public byte[] Serialize<TMessage>(TMessage message) where TMessage : IMessage
    {
        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
    }

    public IMessage Deserialize(byte[] bytes, Type messageType)
    {
        return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(bytes), messageType) as IMessage;
    }
}