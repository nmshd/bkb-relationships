using Enmeshed.BuildingBlocks.Application.Abstractions.Exceptions;
using FluentValidation;
using Relationships.Common;
using Relationships.Common.FluentValidation;

namespace Relationships.Application.RelationshipTemplates.Queries.ListRelationshipTemplates;

// ReSharper disable once UnusedMember.Global
public class ListRelationshipTemplatesValidator : AbstractValidator<ListRelationshipTemplatesQuery>
{
    public ListRelationshipTemplatesValidator()
    {
        RuleFor(query => query.CreatedAt)
            .IsValidRange<ListRelationshipTemplatesQuery, OptionalDateRange, DateTime?>().WithErrorCode(GenericApplicationErrors.Validation.InvalidPropertyValue().Code);
    }
}
