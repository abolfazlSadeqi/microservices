using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.TranscationBases.Queries.GetTranscationBasesWithPagination;

public class GetTranscationBasesWithPaginationQuery : IRequest<PaginatedList<TranscationBaseDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetTranscationBasesWithPaginationQueryHandler : IRequestHandler<GetTranscationBasesWithPaginationQuery, PaginatedList<TranscationBaseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTranscationBasesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TranscationBaseDto>> Handle(GetTranscationBasesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.TranscationBases.AsNoTracking()
            .OrderBy(x => x.CustomerId)
            .ProjectTo<TranscationBaseDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
