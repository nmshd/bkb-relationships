using Enmeshed.BuildingBlocks.Application.Abstractions.Exceptions;

namespace Relationships.Application;

public static class ApplicationErrors
{
    public static class Relationship
    {
        public static ApplicationError MaxNumberOfRelationshipsForTemplateExhausted()
        {
            return new ApplicationError("error.platform.validation.relationship.maxNumberOfRelationshipsForTemplateExhausted", "The maximum number of relationships for the given template is already exhausted.");
        }

        public static ApplicationError RelationshipToTargetAlreadyExists(string targetIdentity = "")
        {
            var targetIdentityString = string.IsNullOrEmpty(targetIdentity) ? "the target identity" : targetIdentity;

            return new ApplicationError("error.platform.validation.relationshipRequest.relationshipToTargetAlreadyExists", $"A relationship to {targetIdentityString} already exists.");
        }

        public static ApplicationError CannotSendRelationshipRequestToYourself()
        {
            return new ApplicationError("error.platform.validation.relationshipRequest.cannotSendRelationshipRequestToYourself", "The template you provided is your own. You cannot send a relationship request to yourself.");
        }
    }
}
