using Application.Interfaces.GenericRepositoryServices;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.RepositoryServices;

public interface IDeviceReadRepository : IReadRepository<Device, Guid>
{
}

public interface IDeviceWriteRepository : IWriteRepository<Device, Guid>
{
}


public interface IEnergyDataReadRepository : IReadRepository<EnergyData, Guid>
{
}

public interface IEnergyDataWriteRepository : IWriteRepository<EnergyData, Guid>
{
}


public interface IGatewayReadRepository : IReadRepository<Gateway, Guid>
{
}

public interface IGatewayWriteRepository : IWriteRepository<Gateway, Guid>
{
}
