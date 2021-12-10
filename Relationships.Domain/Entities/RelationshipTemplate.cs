using Enmeshed.DevelopmentKit.Identity.ValueObjects;
using Enmeshed.Tooling;
using Relationships.Domain.Ids;

namespace Relationships.Domain.Entities;

public class RelationshipTemplate
{
#pragma warning disable CS8618
    private RelationshipTemplate() { }
#pragma warning restore CS8618

    public RelationshipTemplate(IdentityAddress createdBy, DeviceId createdByDevice, int? maxNumberOfRelationships, DateTime? expiresAt, byte[] content)
    {
        Id = RelationshipTemplateId.New();
        CreatedAt = SystemTime.UtcNow;

        CreatedBy = createdBy;
        CreatedByDevice = createdByDevice;
        MaxNumberOfRelationships = maxNumberOfRelationships;
        ExpiresAt = expiresAt;
        Content = content;
    }

    public RelationshipTemplateId Id { get; set; }

    public ICollection<Relationship> Relationships { get; set; } = new List<Relationship>();

    public IdentityAddress CreatedBy { get; set; }
    public DeviceId CreatedByDevice { get; set; }
    public int? MaxNumberOfRelationships { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public byte[]? Content { get; private set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public void LoadContent(byte[] content)
    {
        if (Content != null)
            throw new Exception("Cannot change the content of a relationship template.");

        Content = content;
    }
}
