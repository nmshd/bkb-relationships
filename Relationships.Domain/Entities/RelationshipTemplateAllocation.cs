using Enmeshed.DevelopmentKit.Identity.Entities;
using Enmeshed.DevelopmentKit.Identity.ValueObjects;
using Enmeshed.Tooling;
using Relationships.Domain.Ids;

namespace Relationships.Domain.Entities;

public class RelationshipTemplateAllocation
{
    public RelationshipTemplateAllocation(RelationshipTemplateId templateId, IdentityAddress allocatedBy, DeviceId allocatedByDevice)
    {
        RelationshipTemplateId = templateId;
        AllocatedAt = SystemTime.UtcNow;
        AllocatedBy = allocatedBy;
        AllocatedByDevice = allocatedByDevice;
    }

    public RelationshipTemplateId RelationshipTemplateId{ get; set; }
    public IdentityAddress AllocatedBy { get; set; }
    public DateTime AllocatedAt { get; set; }
    public DeviceId AllocatedByDevice { get; set; }
}
