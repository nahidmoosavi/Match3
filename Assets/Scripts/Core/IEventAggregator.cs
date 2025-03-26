using System;


namespace Core
{
    public interface IEventAggregator
    {
        void Subscribe<T>(Action<T> action);
        void Publish<T>(T message);
    }
}

