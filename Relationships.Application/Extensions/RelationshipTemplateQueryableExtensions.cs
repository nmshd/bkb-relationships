using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Enmeshed.BuildingBlocks.Application.Abstractions.Exceptions;
using Enmeshed.DevelopmentKit.Identity.ValueObjects;
using Enmeshed.Tooling;
using Microsoft.EntityFrameworkCore;
using Relationships.Common;
using Relationships.Domain.Entities;
using Relationships.Domain.Ids;

namespace Relationships.Application.Extensions
{
    public static class RelationshipTemplateQueryableExtensions
    {
        public static async Task<RelationshipTemplate> FirstWithId(this IQueryable<RelationshipTemplate> query, RelationshipTemplateId templateId, CancellationToken cancellationToken)
        {
            var template = await query.FirstOrDefaultAsync(r => r.Id == templateId, cancellationToken);

            if (template == null)
                throw new NotFoundException(nameof(RelationshipTemplate));

            return template;
        }

        public static IQueryable<RelationshipTemplate> WithOwner(this IQueryable<RelationshipTemplate> query, IdentityAddress identityId)
        {
            return query.Where(t => t.CreatedBy == identityId);
        }

        public static IQueryable<RelationshipTemplate> NotExpiredFor(this IQueryable<RelationshipTemplate> query, IdentityAddress address)
        {
            return query.Where(
                t => t.ExpiresAt == null ||
                     t.ExpiresAt > SystemTime.UtcNow ||
                     t.Relationships.Any(r => r.From == address || r.To == address)
            );
        }

        public static IQueryable<RelationshipTemplate> WithIdIn(this IQueryable<RelationshipTemplate> query, IEnumerable<RelationshipTemplateId> ids)
        {
            return query.Where(t => ids.Contains(t.Id));
        }

        public static IQueryable<RelationshipTemplate> NotDeleted(this IQueryable<RelationshipTemplate> query)
        {
            return query.Where(t => !t.DeletedAt.HasValue);
        }

        public static IQueryable<RelationshipTemplate> CreatedAt(this IQueryable<RelationshipTemplate> query, OptionalDateRange createdAt)
        {
            var newQuery = query;

            if (createdAt != null)
            {
                if (createdAt.From != default)
                    newQuery = newQuery.Where(r => r.CreatedAt >= createdAt.From);

                if (createdAt.To != default)
                    newQuery = newQuery.Where(r => r.CreatedAt <= createdAt.To);
            }

            return newQuery;
        }
    }
}
