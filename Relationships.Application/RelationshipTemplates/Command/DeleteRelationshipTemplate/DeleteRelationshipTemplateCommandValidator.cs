using Enmeshed.BuildingBlocks.Application.FluentValidation;
using FluentValidation;

namespace Relationships.Application.RelationshipTemplates.Command.DeleteRelationshipTemplate;

public class DeleteRelationshipTemplateCommandValidator : AbstractValidator<DeleteRelationshipTemplateCommand>
{
    public DeleteRelationshipTemplateCommandValidator()
    {
        RuleFor(c => c.Id).DetailedNotNull();
    }
}