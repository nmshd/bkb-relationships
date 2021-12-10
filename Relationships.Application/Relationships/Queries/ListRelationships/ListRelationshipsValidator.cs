using Enmeshed.BuildingBlocks.Application.Abstractions.Exceptions;
using FluentValidation;
using Relationships.Common;
using Relationships.Common.FluentValidation;

namespace Relationships.Application.Relationships.Queries.ListRelationships
{
    public class ListRelationshipsValidator : AbstractValidator<ListRelationshipsQuery>
    {
        public ListRelationshipsValidator()
        {
            RuleFor(query => query.CreatedAt)
                .IsValidRange<ListRelationshipsQuery, OptionalDateRange, DateTime?>().WithErrorCode(GenericApplicationErrors.Validation.InvalidPropertyValue().Code);
        }
    }
}
