using Application.Interfaces.RepositoryServices;
using Application.Interfaces.RepositoryServices.MenuRepositories;
using Domain;
using Persistence.Contexts;
using Persistence.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories;

public class DeviceReadRepository : ReadRepository<Device, Guid, BaseDbContext>, IDeviceReadRepository
{
    public DeviceReadRepository(BaseDbContext context) : base(context)
    {
    }
}


public class DeviceWriteRepository : WriteRepository<Device, Guid, BaseDbContext>, IDeviceWriteRepository
{
    public DeviceWriteRepository(BaseDbContext context) : base(context)
    {
    }
}


public class GatewayReadRepository : ReadRepository<Gateway, Guid, BaseDbContext>, IGatewayReadRepository
{
    public GatewayReadRepository(BaseDbContext context) : base(context)
    {
    }
}


public class GatewayWriteRepository : WriteRepository<Gateway, Guid, BaseDbContext>, IGatewayWriteRepository
{
    public GatewayWriteRepository(BaseDbContext context) : base(context)
    {
    }
}


public class EnergyDataReadRepository : ReadRepository<EnergyData, Guid, BaseDbContext>, IEnergyDataReadRepository
{
    public EnergyDataReadRepository(BaseDbContext context) : base(context)
    {
    }
}


public class EnergyDataWriteRepository : WriteRepository<EnergyData, Guid, BaseDbContext>, IEnergyDataWriteRepository
{
    public EnergyDataWriteRepository(BaseDbContext context) : base(context)
    {
    }
}
