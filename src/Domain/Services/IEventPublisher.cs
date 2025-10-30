using System;

namespace Domain.Services
{
    public interface IEventPublisher
    {
        void Publish(object @event);
    }
}