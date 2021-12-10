﻿using AutoMapper;
using Enmeshed.BuildingBlocks.Application.Abstractions.Exceptions;
using Enmeshed.BuildingBlocks.Application.Abstractions.Infrastructure.EventBus;
using Enmeshed.BuildingBlocks.Application.Abstractions.Infrastructure.Persistence.Database;
using Enmeshed.BuildingBlocks.Application.Abstractions.Infrastructure.UserContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Relationships.Application.Extensions;
using Relationships.Application.Infrastructure;
using Relationships.Application.IntegrationEvents;
using Relationships.Domain.Entities;
using Relationships.Domain.Ids;

namespace Relationships.Application.Relationships.Commands.CreateRelationship;

public class Handler : IRequestHandler<CreateRelationshipCommand, CreateRelationshipResponse>
{
    private readonly IContentStore _contentStore;
    private readonly IDbContext _dbContext;
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private CancellationToken _cancellationToken;
    private Relationship _relationship;
    private CreateRelationshipCommand _request;
    private RelationshipTemplate _template;

    public Handler(IDbContext dbContext, IUserContext userContext, IMapper mapper, IEventBus eventBus, IContentStore contentStore)
    {
        _dbContext = dbContext;
        _userContext = userContext;
        _mapper = mapper;
        _eventBus = eventBus;
        _contentStore = contentStore;
    }


    public async Task<CreateRelationshipResponse> Handle(CreateRelationshipCommand request, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        _request = request;

        await ReadTemplateFromDb();
        await EnsureRelationshipCanBeEstablished();
        await CreateAndSaveRelationship();
        PublishIntegrationEvent();

        return CreateResponse();
    }

    private async Task ReadTemplateFromDb()
    {
        _template = await _dbContext
            .Set<RelationshipTemplate>()
            .NotDeleted()
            .NotExpiredFor(_userContext.GetAddress())
            .FirstWithId(_request.RelationshipTemplateId, _cancellationToken);
    }

    private async Task EnsureRelationshipCanBeEstablished()
    {
        EnsureActiveIdentityIsNotTemplateOwner();
        await EnsureMaxNumberOfRelationshipsIsNotExhausted();
        await EnsureThereIsNoExistingRelationshipBetweenActiveIdentityAndTemplateOwner();
    }

    private void EnsureActiveIdentityIsNotTemplateOwner()
    {
        if (_template.CreatedBy == _userContext.GetAddress())
            throw new OperationFailedException(ApplicationErrors.Relationship.CannotSendRelationshipRequestToYourself());
    }

    private async Task EnsureMaxNumberOfRelationshipsIsNotExhausted()
    {
        if (_template.MaxNumberOfRelationships.HasValue && await GetNumberOfRelationshipsOfTemplate(_request.RelationshipTemplateId) >= _template.MaxNumberOfRelationships)
            throw new OperationFailedException(ApplicationErrors.Relationship.MaxNumberOfRelationshipsForTemplateExhausted());
    }

    private async Task EnsureThereIsNoExistingRelationshipBetweenActiveIdentityAndTemplateOwner()
    {
        var relationshipExists = await _dbContext
            .SetReadOnly<Relationship>()
            .BetweenParticipants(_userContext.GetAddress(), _template.CreatedBy)
            .Where(r => r.Status != RelationshipStatus.Terminated && r.Status != RelationshipStatus.Rejected && r.Status != RelationshipStatus.Revoked)
            .AnyAsync(_cancellationToken);

        if (relationshipExists)
            throw new OperationFailedException(ApplicationErrors.Relationship.RelationshipToTargetAlreadyExists(_template.CreatedBy));
    }

    private async Task CreateAndSaveRelationship()
    {
        _relationship = new Relationship(
            _template,
            _userContext.GetAddress(),
            _userContext.GetDeviceId(),
            _request.Content);

        await _contentStore.SaveContentOfChangeRequest(_relationship.Changes.GetLatestOfType(RelationshipChangeType.Creation).Request);

        await _dbContext.Set<Relationship>().AddAsync(_relationship, _cancellationToken);

        try
        {
            await _dbContext.SaveChangesAsync(_cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains(ConstraintNames.ONLY_ONE_ACTIVE_RELATIONSHIP_BETWEEN_TWO_IDENTITIES))
                throw new OperationFailedException(ApplicationErrors.Relationship.RelationshipToTargetAlreadyExists(_template.CreatedBy));

            throw;
        }
    }

    private void PublishIntegrationEvent()
    {
        var change = _relationship.Changes.FirstOrDefault();
        var evt = new RelationshipChangeCreatedIntegrationEvent(change);
        _eventBus.Publish(evt);
    }

    private CreateRelationshipResponse CreateResponse()
    {
        return _mapper.Map<CreateRelationshipResponse>(_relationship);
    }

    private async Task<int> GetNumberOfRelationshipsOfTemplate(RelationshipTemplateId id)
    {
        return await _dbContext
            .SetReadOnly<Relationship>()
            .CountAsync(r => r.RelationshipTemplateId == id, _cancellationToken);
    }
}