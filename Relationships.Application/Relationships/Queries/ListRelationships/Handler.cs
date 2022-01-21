using AutoMapper;
using Enmeshed.BuildingBlocks.Application.Abstractions.Infrastructure.Persistence.Database;
using Enmeshed.BuildingBlocks.Application.Abstractions.Infrastructure.UserContext;
using Enmeshed.BuildingBlocks.Application.Extensions;
using Microsoft.EntityFrameworkCore;
using Relationships.Application.Extensions;
using Relationships.Application.Infrastructure;
using Relationships.Application.Relationships.DTOs;
using Relationships.Domain.Entities;

namespace Relationships.Application.Relationships.Queries.ListRelationships;

public class Handler : RequestHandlerBase<ListRelationshipsQuery, ListRelationshipsResponse>
{
    private readonly IContentStore _contentStore;

    public Handler(IDbContext dbContext, IUserContext userContext, IMapper mapper, IContentStore contentStore) : base(dbContext, userContext, mapper)
    {
        _contentStore = contentStore;
    }

    public override async Task<ListRelationshipsResponse> Handle(ListRelationshipsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext
            .Set<Relationship>()
            .IncludeAll()
            .AsNoTracking()
            .WithParticipant(_activeIdentity);

        if (request.Ids.Any())
            query = query.WithIdIn(request.Ids);

        if (request.CreatedAt != null)
            query = query.CreatedAt(request.CreatedAt);
        
        var items = await query
            .OrderBy(d => d.CreatedAt)
            .Paged(request.PaginationFilter)
            .ToListAsync(cancellationToken);

        var totalNumberOfItems = items.Count < request.PaginationFilter.PageSize ? items.Count : await query.CountAsync(cancellationToken);

        var changes = items.SelectMany(r => r.Changes);

        await _contentStore.FillContentOfChanges(changes);

        return new ListRelationshipsResponse(items.Select(r => _mapper.Map<RelationshipDTO>(r)), request.PaginationFilter, totalNumberOfItems);
    }
}
