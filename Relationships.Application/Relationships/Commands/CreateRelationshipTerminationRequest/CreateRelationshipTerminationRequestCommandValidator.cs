using Enmeshed.BuildingBlocks.Application.FluentValidation;
using FluentValidation;

namespace Relationships.Application.Relationships.Commands.CreateRelationshipTerminationRequest;

public class CreateRelationshipTerminationRequestCommandValidator : AbstractValidator<CreateRelationshipTerminationRequestCommand>
{
    public CreateRelationshipTerminationRequestCommandValidator()
    {
        RuleFor(c => c.Id).DetailedNotNull();
    }
}