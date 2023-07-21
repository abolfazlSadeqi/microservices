using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entity;


public class TranscationBase : AuditableEntity, IHasDomainEvent
{

    public int Id { get; set; }

    public int CustomerId { get; set; }
    public long Amount { get; set; }

    public DateTime Date { get; set; }

    public string DeviceType { get; set; }


    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}
