using System;
using System.Collections.Generic;

namespace Core
{
    public sealed class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

        public void Subscribe<T>(Action<T> action)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
                _subscribers[type] = new List<Delegate>();
            _subscribers[type].Add(action);
        }

        public void Publish<T>(T message)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type)) return;
            foreach (var action in _subscribers[type])
                ((Action<T>)action)(message);
        }
    }
}
