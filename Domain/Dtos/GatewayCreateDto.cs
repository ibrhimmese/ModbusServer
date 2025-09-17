namespace Domain.Dtos;

public class GatewayCreateDto
{
    public string IpAddress { get; set; } = string.Empty;
    public int Port { get; set; }
}

public class GatewayUpdateDto
{
    public string IpAddress { get; set; } = string.Empty;
    public int Port { get; set; }
}

public class GatewayReadDto
{
    public Guid Id { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public int Port { get; set; }
    public List<DeviceReadDto> Devices { get; set; } = new();
}

public class DeviceCreateDto
{
    public int UnitId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public Guid GatewayId { get; set; }
}

public class DeviceUpdateDto
{
    public int UnitId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public Guid GatewayId { get; set; }
}

public class DeviceReadDto
{
    public Guid Id { get; set; }
    public int UnitId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}

public class EnergyDataReadDto
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string GatewayIp { get; set; } = string.Empty;
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