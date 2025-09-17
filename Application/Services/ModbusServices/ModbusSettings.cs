namespace Application.Services.ModbusServices;

public class ModbusSettings
{
    public int TimeoutMs { get; set; } = 5000;
    public int RetryCount { get; set; } = 2;
    public int InterDeviceDelayMs { get; set; } = 50;
    public int InterRequestDelayMs { get; set; } = 10;
}
