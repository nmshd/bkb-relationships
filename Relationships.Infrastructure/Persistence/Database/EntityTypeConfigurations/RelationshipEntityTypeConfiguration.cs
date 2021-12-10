using Enmeshed.DevelopmentKit.Identity.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Relationships.Domain.Entities;
using Relationships.Domain.Ids;

namespace Relationships.Infrastructure.Persistence.Database.EntityTypeConfigurations;

public class RelationshipEntityTypeConfiguration : IEntityTypeConfiguration<Relationship>
{
    public void Configure(EntityTypeBuilder<Relationship> builder)
    {
        builder.HasIndex(x => x.From);
        builder.HasIndex(x => x.To);
        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.Status);

        builder.Metadata.FindNavigation(nameof(Relationship.Changes)).SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(x => x.Id).HasColumnType($"char({RelationshipId.MAX_LENGTH})");
        builder.Property(x => x.From).HasColumnType($"char({IdentityAddress.MAX_LENGTH})");
        builder.Property(x => x.To).HasColumnType($"char({IdentityAddress.MAX_LENGTH})");
    }
}