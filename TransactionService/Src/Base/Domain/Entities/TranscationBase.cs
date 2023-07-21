using Domain.Common;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Common.Validation;
using Common.Other;
using System.Runtime.CompilerServices;

namespace Domain.Entities;


public class TranscationBase : AuditableEntity, IHasDomainEvent
{

    public TranscationBase CreateCustomer(
        int CustomerId,
        long Amount,
        DateTime Date
        , string DeviceType
        )
    {
        TranscationBase customer = new();
        customer.CustomerId = CustomerId;
        customer.Amount = Amount;
        customer.Date = Date;
        customer.DeviceType = DeviceType;
        return customer;
    }


    public TranscationBase UpdateCustomer( TranscationBase _Customer,
        int CustomerId,
        long Amount,
        DateTime Date
        , string DeviceType
        )
    {
        _Customer.CustomerId = CustomerId;
        _Customer.Amount = Amount;
        _Customer.Date = Date;
        _Customer.DeviceType = DeviceType;
      
        return _Customer;
    }


    public int Id { get; set; }


    public int CustomerId { get; set; }
    public long Amount { get; set; }

    public DateTime Date { get; set; }

    public string DeviceType { get; set; }



    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}