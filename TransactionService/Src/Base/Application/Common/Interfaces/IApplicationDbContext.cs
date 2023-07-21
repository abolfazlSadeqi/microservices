using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Domain.Entities.TranscationBase> TranscationBases { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
