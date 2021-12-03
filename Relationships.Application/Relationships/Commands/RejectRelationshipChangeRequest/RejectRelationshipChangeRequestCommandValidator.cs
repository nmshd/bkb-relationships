using Enmeshed.BuildingBlocks.Application.FluentValidation;
using FluentValidation;

namespace Relationships.Application.Relationships.Commands.RejectRelationshipChangeRequest
{
    public class RejectRelationshipChangeRequestCommandValidator : AbstractValidator<RejectRelationshipChangeRequestCommand>
    {
        public RejectRelationshipChangeRequestCommandValidator()
        {
            RuleFor(c => c.Id).DetailedNotNull();
            RuleFor(c => c.ResponseContent).NumberOfBytes(0, 10 * 1024 * 1024);
        }
    }
}
