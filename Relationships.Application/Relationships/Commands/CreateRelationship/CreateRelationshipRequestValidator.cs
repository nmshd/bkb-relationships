using Enmeshed.BuildingBlocks.Application.FluentValidation;
using FluentValidation;

namespace Relationships.Application.Relationships.Commands.CreateRelationship
{
    public class CreateRelationshipCommandValidator : AbstractValidator<CreateRelationshipCommand>
    {
        public CreateRelationshipCommandValidator()
        {
            RuleFor(c => c.RelationshipTemplateId).DetailedNotEmpty();
            RuleFor(c => c.Content).NumberOfBytes(0, 10 * 1024 * 1024);
        }
    }
}
