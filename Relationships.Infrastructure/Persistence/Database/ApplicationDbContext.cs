using Enmeshed.BuildingBlocks.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Relationships.Domain.Entities;
using Relationships.Domain.Ids;
using Relationships.Infrastructure.Persistence.Database.ValueConverters;

namespace Relationships.Infrastructure.Persistence.Database;

public class ApplicationDbContext : AbstractDbContextBase
{
    public ApplicationDbContext() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Relationship> Relationships { get; set; }
    public DbSet<RelationshipChange> RelationshipChanges { get; set; }
    public DbSet<RelationshipTemplate> RelationshipTemplates { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.UseValueConverter(new RelationshipIdEntityFrameworkValueConverter(new ConverterMappingHints(RelationshipId.MAX_LENGTH)));
        builder.UseValueConverter(new RelationshipTemplateIdEntityFrameworkValueConverter(new ConverterMappingHints(RelationshipId.MAX_LENGTH)));

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}