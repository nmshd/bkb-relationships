using Enmeshed.BuildingBlocks.Application.FluentValidation;
using FluentValidation;

namespace Relationships.Application.Relationships.Commands.AcceptRelationshipChangeRequest
{
    public class AcceptRelationshipChangeRequestCommandValidator : AbstractValidator<AcceptRelationshipChangeRequestCommand>
    {
        public AcceptRelationshipChangeRequestCommandValidator()
        {
            RuleFor(c => c.Id).DetailedNotNull();
            RuleFor(c => c.ResponseContent).NumberOfBytes(0, 10 * 1024 * 1024);
        }
    }
}
