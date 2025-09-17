using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasQueryFilter(b=> !b.DeletedDate.HasValue);
    }
}

public class GatewayConfiguration : IEntityTypeConfiguration<Gateway>
{
    public void Configure(EntityTypeBuilder<Gateway> builder)
    {
        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    }
}

public class EnergyDataConfiguration : IEntityTypeConfiguration<EnergyData>
{
    public void Configure(EntityTypeBuilder<EnergyData> builder)
    {
        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    }
}
