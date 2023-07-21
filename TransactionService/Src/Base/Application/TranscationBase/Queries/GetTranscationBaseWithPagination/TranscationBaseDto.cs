using Application.Common.Mappings;
using Domain.Entities;

namespace Application.TranscationBases.Queries.GetTranscationBasesWithPagination;

public class TranscationBaseDto : IMapFrom<TranscationBase>
{
   
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public long Amount { get; set; }

    public DateTime Date { get; set; }

    public string DeviceType { get; set; }

    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
    public Guid RowVersion { get; set; }
}
