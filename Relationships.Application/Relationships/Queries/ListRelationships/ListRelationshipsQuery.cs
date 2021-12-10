using Enmeshed.BuildingBlocks.Application.Pagination;
using MediatR;
using Relationships.Common;
using Relationships.Domain.Ids;

namespace Relationships.Application.Relationships.Queries.ListRelationships
{
    public class ListRelationshipsQuery : IRequest<ListRelationshipsResponse>
    {
        public ListRelationshipsQuery(PaginationFilter paginationFilter, OptionalDateRange createdAt, IEnumerable<RelationshipId> ids)
        {
            PaginationFilter = paginationFilter;
            CreatedAt = createdAt;
            Ids = ids;
        }

        public PaginationFilter PaginationFilter { get; set; }
        public OptionalDateRange CreatedAt { get; set; }
        public IEnumerable<RelationshipId> Ids { get; set; }
    }
}
