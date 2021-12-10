using Enmeshed.BuildingBlocks.Application.Pagination;
using MediatR;
using Relationships.Common;
using Relationships.Domain.Ids;

namespace Relationships.Application.RelationshipTemplates.Queries.ListRelationshipTemplates
{
    public class ListRelationshipTemplatesQuery : IRequest<ListRelationshipTemplatesResponse>
    {
        public ListRelationshipTemplatesQuery(PaginationFilter paginationFilter, OptionalDateRange createdAt, IEnumerable<RelationshipTemplateId> ids)
        {
            PaginationFilter = paginationFilter;
            CreatedAt = createdAt;
            Ids = ids;
        }

        public PaginationFilter PaginationFilter { get; set; }
        public OptionalDateRange CreatedAt { get; set; }
        public IEnumerable<RelationshipTemplateId> Ids { get; set; }
    }
}
