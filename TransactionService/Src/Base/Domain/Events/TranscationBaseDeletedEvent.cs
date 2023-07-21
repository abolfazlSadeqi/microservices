using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public class TranscationBaseDeletedEvent : DomainEvent
{
    public TranscationBaseDeletedEvent(TranscationBase item)
    {
        Item = item;
    }

    public TranscationBase Item { get; }
}
