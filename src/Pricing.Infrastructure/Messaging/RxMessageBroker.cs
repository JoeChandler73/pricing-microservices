using System.Reactive.Linq;
using System.Reactive.Subjects;
using Pricing.Application.MessageHandlers;
using Pricing.Application.Messages;
using Pricing.Application.Messaging;

namespace Pricing.Infrastructure.Messaging;

public class RxMessageBroker : IMessageBroker
{
    private readonly Subject<IMessage> _subject = new();
    private readonly List<IDisposable> _subscriptions = new();

    public Task Publish<T>(T message) where T : IMessage
    {
        _subject.OnNext(message);
        return Task.CompletedTask;
    }

    public Task Subscribe<T>(IMessageHandler<T> handler) where T : IMessage
    {
        _subscriptions.Add(_subject.OfType<T>().Subscribe(async (message) => await handler.HandleAsync(message)));
        return Task.CompletedTask;
    }

    public Task Subscribe<T>(Func<T, Task> handler) where T : IMessage
    {
        _subscriptions.Add(_subject.OfType<T>().Subscribe(async (message) => await handler(message)));
        return Task.CompletedTask;
    }

    public Task HandleAsync(IMessage message)
    {
        return Publish(message);
    }

    public void Dispose()
    {
        _subscriptions.ForEach(x => x.Dispose());
        _subject.Dispose();
    }
}