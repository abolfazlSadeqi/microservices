using Application.Common.Interfaces;
using Application.TranscationBases.Queries.GetTranscationBasesWithPagination;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.TranscationBases.Queries.GetTranscationBasesWithPagination;

public class GetTranscationBaseQuery : IRequest<TranscationBaseDto>
{
    public int Id { get; set; }
}

public class GetCustomerQueryHandler : IRequestHandler<GetTranscationBaseQuery, TranscationBaseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomerQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TranscationBaseDto> Handle(GetTranscationBaseQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.TranscationBases.FindAsync(request.Id);

        return _mapper.Map<TranscationBase, TranscationBaseDto>(entity!);
    }
}

