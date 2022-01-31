﻿using System.Threading.Tasks;

namespace EventDriven.EventBus.Abstractions.Tests.Fakes
{
    public class FakeCachingMessageBroker : FakeMessageBroker
    {
        private readonly IEventCache _eventCache;

        public IEventBus EventBus { get; set; }

        public FakeCachingMessageBroker(
            IEventCache eventCache)
        {
            _eventCache = eventCache;
        }
        
        public override Task PublishEventAsync<TIntegrationEvent>(
            TIntegrationEvent @event,
            string topic)
        {
            var handlers = Topics[topic];
            if (handlers == null) return Task.CompletedTask;
            foreach (var handler in handlers)
            {
                if (_eventCache.TryAdd(@event))
                    handler.HandleAsync(@event);
            }
            return Task.CompletedTask;
        }
    }
}