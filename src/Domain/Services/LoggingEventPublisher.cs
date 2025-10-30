using System;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class LoggingEventPublisher : IEventPublisher
    {
        private readonly ILogger<LoggingEventPublisher> _logger;
        public LoggingEventPublisher(ILogger<LoggingEventPublisher> logger)
        {
            _logger = logger;
        }

        public void Publish(object @event)
        {
            // In a real system this would publish to a broker. For prototype, log it.
            _logger.LogInformation("Event published: {Event}", @event);
        }
    }
}