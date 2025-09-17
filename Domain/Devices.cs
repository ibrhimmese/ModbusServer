using Domain.BaseProjeEntities.CoreEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;

public class Device:Entity<Guid>
{
    public int UnitId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public Guid GatewayId { get; set; }
    public Gateway Gateway { get; set; } = null!;
}


public class EnergyData : Entity<Guid>
{
    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;
    public DateTime Timestamp { get; set; }

    public float Voltage { get; set; }
    public float Current { get; set; }
    public float Power { get; set; }
    public uint Energy { get; set; }
    public float Frequency { get; set; }
    public float PowerFactor { get; set; }
    public byte Status { get; set; }
    public byte LoadPercentage { get; set; }
}


public class Gateway : Entity<Guid>
{
    public string IpAddress { get; set; } = string.Empty;
    public int Port { get; set; }

    public List<Device> Devices { get; set; } = new();
}
