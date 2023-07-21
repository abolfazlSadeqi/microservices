using Application.Common.Models;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.TranscationBases.EventHandlers;

public class TranscationBaseCreatedEventHandler : INotificationHandler<DomainEventNotification<TranscationBaseCreatedEvent>>
{
    private readonly ILogger<TranscationBaseCreatedEventHandler> _logger;

    public TranscationBaseCreatedEventHandler(ILogger<TranscationBaseCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<TranscationBaseCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

        return Task.CompletedTask;
    }
}