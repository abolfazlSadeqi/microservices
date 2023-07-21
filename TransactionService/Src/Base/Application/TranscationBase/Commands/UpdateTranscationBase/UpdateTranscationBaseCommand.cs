using Common.Exceptions;
using Application.Common.Interfaces;
using Application.Customers.Commands.CreateCustomer;
using Domain.Entities;
using Domain.Events;
using Domain.ValueObjects;
using MediatR;

namespace Application.TranscationBases.Commands.UpdateTranscationBase;

public class UpdateTranscationBaseCommand : IRequest<int>
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    public long Amount { get; set; }

    public DateTime Date { get; set; }

    public string DeviceType { get; set; }
}

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateTranscationBaseCommand,int>
{
    private readonly IApplicationDbContext _context;

    public UpdateCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }


  
    public async Task<int> Handle(UpdateTranscationBaseCommand request, CancellationToken cancellationToken)
    {


        UpdateTranscationBaseCommandLogicValidator validationsLogic = new UpdateTranscationBaseCommandLogicValidator();
        validationsLogic.IsValidCustomers(request);


        var entity = await _context.TranscationBases
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TranscationBase), request.Id);
        }

        entity.UpdateCustomer(entity,

             request.CustomerId, request.Amount, request.Date,
             request.DeviceType
          );

        

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

