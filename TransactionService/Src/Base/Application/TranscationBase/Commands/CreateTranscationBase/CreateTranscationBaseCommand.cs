using Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.ValueObjects;
using Common.Other;
using MediatR;

namespace Application.Customers.Commands.CreateCustomer;

public class CreateTranscationBaseCommand : IRequest<int>
{

    public int CustomerId { get; set; }
    public long Amount { get; set; }

    public DateTime Date { get; set; }

    public string DeviceType { get; set; }

}

public class CreateCustomerCommandHandler : IRequestHandler<CreateTranscationBaseCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTranscationBaseCommand request, CancellationToken cancellationToken)
    {
        UpdateCustomerCommandLogicValidator validationsLogic = new UpdateCustomerCommandLogicValidator();
        validationsLogic.IsValidCustomers(request);

        TranscationBase entity = new();
        entity = entity.CreateCustomer(

           request.CustomerId, request.Amount, request.Date,
           request.DeviceType
        );

       // validationsLogic.IsValidEmailunique(_context, entity);
       

        entity.DomainEvents.Add(new TranscationBaseCreatedEvent(entity));

        _context.TranscationBases.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}