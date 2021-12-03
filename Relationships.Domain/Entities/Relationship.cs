using System;
using System.Linq;
using Enmeshed.DevelopmentKit.Identity.ValueObjects;
using Relationships.Domain.Errors;
using Relationships.Domain.Ids;
using Enmeshed.Tooling;

namespace Relationships.Domain.Entities
{
    public class Relationship
    {
        private readonly RelationshipChangeLog _changes = new();

#pragma warning disable CS8618
        private Relationship() { }
#pragma warning restore CS8618

        public Relationship(RelationshipTemplate relationshipTemplate, IdentityAddress from, DeviceId fromDevice, byte[] requestContent)
        {
            Id = RelationshipId.New();
            RelationshipTemplateId = relationshipTemplate.Id;
            RelationshipTemplate = relationshipTemplate;

            From = from;
            To = relationshipTemplate.CreatedBy;
            Status = RelationshipStatus.Pending;

            CreatedAt = SystemTime.UtcNow;

            _changes.Add(new RelationshipCreationChange(this, from, fromDevice, requestContent));
        }

        public RelationshipId Id { get; }
        public RelationshipTemplateId? RelationshipTemplateId { get; }
        public RelationshipTemplate? RelationshipTemplate { get; }

        public IdentityAddress From { get; }
        public IdentityAddress To { get; }
        public IRelationshipChangeLog Changes => _changes;

        public DateTime CreatedAt { get; }

        public RelationshipStatus Status { get; private set; }

        public RelationshipChange AcceptChange(RelationshipChangeId changeId, IdentityAddress acceptedBy, DeviceId acceptedByDevice, byte[]? content)
        {
            var change = _changes.GetById(changeId);

            change.Accept(acceptedBy, acceptedByDevice, content);
            Status = GetStatusAfterChange(change);

            return change;
        }

        public RelationshipChange RejectChange(RelationshipChangeId changeId, IdentityAddress rejectedBy, DeviceId rejectedByDevice, byte[]? content)
        {
            var change = _changes.GetById(changeId);

            if (change.IsCompleted)
                throw new DomainException(DomainErrors.ChangeRequestIsAlreadyCompleted(change.Status));

            if (rejectedBy == change.Request.CreatedBy)
                throw new DomainException(DomainErrors.ChangeRequestCannotBeRejectedByCreator());

            change.Reject(rejectedBy, rejectedByDevice, content);
            Status = GetStatusAfterChange(change);

            return change;
        }

        public RelationshipChange RevokeChange(RelationshipChangeId changeId, IdentityAddress revokedBy, DeviceId revokedByDevice, byte[]? content)
        {
            var change = _changes.GetById(changeId);

            if (change.IsCompleted)
                throw new DomainException(DomainErrors.ChangeRequestIsAlreadyCompleted(change.Status));

            if (revokedBy != change.Request.CreatedBy)
                throw new DomainException(DomainErrors.ChangeRequestCanOnlyBeRevokedByCreator());

            change.Revoke(revokedBy, revokedByDevice, content);
            Status = GetStatusAfterChange(change);

            return change;
        }

        private static RelationshipStatus GetStatusAfterChange(RelationshipChange change)
        {
            switch (change.Type)
            {
                case RelationshipChangeType.Creation:
                    switch (change.Status)
                    {
                        case RelationshipChangeStatus.Accepted:
                            return RelationshipStatus.Active;
                        case RelationshipChangeStatus.Rejected:
                            return RelationshipStatus.Rejected;
                        case RelationshipChangeStatus.Revoked:
                            return RelationshipStatus.Revoked;
                        default:
                            throw new NotSupportedException();
                    }

                case RelationshipChangeType.Termination:
                    switch (change.Status)
                    {
                        case RelationshipChangeStatus.Accepted:
                            return RelationshipStatus.Terminated;
                        case RelationshipChangeStatus.Rejected:
                            return RelationshipStatus.Active;
                        case RelationshipChangeStatus.Revoked:
                            return RelationshipStatus.Active;
                        default:
                            throw new NotSupportedException();
                    }
                case RelationshipChangeType.TerminationCancellation:
                    throw new NotImplementedException();
                default:
                    throw new NotSupportedException();
            }
        }

        private RelationshipChange? GetPendingChangeOrNull()
        {
            return _changes.FirstOrDefault(c => c.Status == RelationshipChangeStatus.Pending);
        }

        public RelationshipChange RequestTermination(IdentityAddress requestedBy, DeviceId requestedByDevice)
        {
            EnsureCanBeTerminated(requestedBy, requestedByDevice);

            var terminationChange = new RelationshipTerminationChange(this, requestedBy, requestedByDevice);
            _changes.Add(terminationChange);

            return terminationChange;
        }

        private void EnsureCanBeTerminated(IdentityAddress requestedBy, DeviceId requestedByDevice)
        {
            if (Status != RelationshipStatus.Active)
                throw new DomainException(DomainErrors.OnlyActiveRelationshipsCanBeTerminated());

            var existingChange = GetPendingChangeOrNull();

            if (existingChange != null)
                throw new DomainException(DomainErrors.PendingChangeAlreadyExisits(existingChange.Id));
        }
    }
}
