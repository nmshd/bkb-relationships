using AutoMapper;
using Enmeshed.BuildingBlocks.Application.Abstractions.Infrastructure.Persistence.Database;
using Enmeshed.BuildingBlocks.Application.Abstractions.Infrastructure.UserContext;
using Enmeshed.BuildingBlocks.Application.Extensions;
using Microsoft.EntityFrameworkCore;
using Relationships.Application.Extensions;
using Relationships.Application.Infrastructure;
using Relationships.Application.Relationships;
using Relationships.Application.Relationships.DTOs;
using Relationships.Domain.Entities;

namespace Relationships.Application.RelationshipTemplates.Queries.ListRelationshipTemplates;

public class Handler : RequestHandlerBase<ListRelationshipTemplatesQuery, ListRelationshipTemplatesResponse>
{
    private readonly IContentStore _contentStore;

    public Handler(IDbContext dbContext, IUserContext userContext, IMapper mapper, IContentStore contentStore) : base(dbContext, userContext, mapper)
    {
        _contentStore = contentStore;
    }

    public override async Task<ListRelationshipTemplatesResponse> Handle(ListRelationshipTemplatesQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext
            .SetReadOnly<RelationshipTemplate>()
            .NotExpiredFor(_activeIdentity)
            .NotDeleted();

        if (request.Ids.Any())
            query = query.WithIdIn(request.Ids);
        else
            query = query.WithOwner(_activeIdentity);

        if (request.CreatedAt != null)
            query = query.CreatedAt(request.CreatedAt);

        var totalRecords = await query.CountAsync(cancellationToken);

        var templates = await query
            .OrderBy(d => d.CreatedAt)
            .Paged(request.PaginationFilter)
            .ToListAsync(cancellationToken);

        await _contentStore.FillContentOfTemplates(templates);

        return new ListRelationshipTemplatesResponse(templates.Select(t => _mapper.Map<RelationshipTemplateDTO>(t)), request.PaginationFilter, totalRecords);
    }
}
