﻿using AutoMapper;
using Enmeshed.BuildingBlocks.Application.Abstractions.Infrastructure.Persistence.BlobStorage;
using Enmeshed.BuildingBlocks.Application.Abstractions.Infrastructure.Persistence.Database;
using Enmeshed.BuildingBlocks.Application.Abstractions.Infrastructure.UserContext;
using Relationships.Application.Extensions;
using Relationships.Application.Relationships;
using Relationships.Application.Relationships.DTOs;
using Relationships.Domain.Entities;

namespace Relationships.Application.RelationshipTemplates.Queries.GetRelationshipTemplate
{
    public class Handler : RequestHandlerBase<GetRelationshipTemplateQuery, RelationshipTemplateDTO>
    {
        private readonly IBlobStorage _blobStorage;

        public Handler(IDbContext dbContext, IUserContext userContext, IMapper mapper, IBlobStorage blobStorage) : base(dbContext, userContext, mapper)
        {
            _blobStorage = blobStorage;
        }

        public override async Task<RelationshipTemplateDTO> Handle(GetRelationshipTemplateQuery request, CancellationToken cancellationToken)
        {
            var template = await _dbContext
                .SetReadOnly<RelationshipTemplate>()
                .NotExpiredFor(_activeIdentity)
                .NotDeleted()
                .FirstWithId(request.Id, cancellationToken);

            template.LoadContent(await _blobStorage.FindAsync(template.Id));

            return _mapper.Map<RelationshipTemplateDTO>(template);
        }
    }
}
