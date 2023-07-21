
using Application.Common.Interfaces;
using Common.Exceptions;
using Domain.Entities;
using Domain.Events;
using MediatR;

namespace Application.Customers.Commands.DeleteCustomer;

public class DeleteTranscationBaseCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteTranscationBaseCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteTranscationBaseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TranscationBases
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TranscationBase), request.Id);
        }

        _context.TranscationBases.Remove(entity);

        entity.DomainEvents.Add(new TranscationBaseDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}