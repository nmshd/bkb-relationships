using Enmeshed.BuildingBlocks.Application.Abstractions.Infrastructure.Mapping;
using MediatR;
using Relationships.Domain.Entities;

namespace Relationships.Application.Relationships.Commands.CreateRelationshipTemplate;

public class CreateRelationshipTemplateCommand : IMapTo<RelationshipTemplate>, IRequest<CreateRelationshipTemplateResponse>
{
    public DateTime? ExpiresAt { get; set; }
    public int? MaxNumberOfRelationships { get; set; }
    public byte[] Content { get; set; }
}
