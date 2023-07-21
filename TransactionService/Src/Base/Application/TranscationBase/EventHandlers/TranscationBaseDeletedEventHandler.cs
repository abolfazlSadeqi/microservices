using Application.Common.Models;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.TranscationBases.EventHandlers;

public class TranscationBaseDeletedEventHandler : INotificationHandler<DomainEventNotification<TranscationBaseDeletedEvent>>
{
    private readonly ILogger<TranscationBaseDeletedEventHandler> _logger;

    public TranscationBaseDeletedEventHandler(ILogger<TranscationBaseDeletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<TranscationBaseDeletedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

        return Task.CompletedTask;
    }
}