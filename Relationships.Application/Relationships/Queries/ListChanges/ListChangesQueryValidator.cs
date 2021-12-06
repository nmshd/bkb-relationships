using System;
using Enmeshed.BuildingBlocks.Application.Abstractions.Exceptions;
using FluentValidation;
using Relationships.Common;
using Relationships.Common.FluentValidation;

namespace Relationships.Application.Relationships.Queries.ListChanges
{
    public class ListChangesQueryValidator : AbstractValidator<ListChangesQuery>
    {
        public ListChangesQueryValidator()
        {
            RuleFor(query => query.CreatedAt)
                .IsValidRange<ListChangesQuery, OptionalDateRange, DateTime?>().WithErrorCode(GenericApplicationErrors.Validation.InvalidPropertyValue().Code);
        }
    }
}
