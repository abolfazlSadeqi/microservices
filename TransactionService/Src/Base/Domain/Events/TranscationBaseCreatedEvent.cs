using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public class TranscationBaseCreatedEvent : DomainEvent
{
    public TranscationBaseCreatedEvent(TranscationBase item)
    {
        Item = item;
    }

    public TranscationBase Item { get; }
}