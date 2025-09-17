using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Application.Interfaces.RepositoryServices;
using Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.ModbusServices;

public class EnergyDataService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<EnergyDataService> _logger;
    private readonly ModbusSettings _settings;
    private readonly IGatewayReadRepository _gatewayReadRepository;

    public EnergyDataService(IServiceProvider services,
                           ILogger<EnergyDataService> logger,
                           IOptions<ModbusSettings> settings,
                           IGatewayReadRepository gatewayReadRepository)
    {
        _services = services;
        _logger = logger;
        _settings = settings.Value;
        _gatewayReadRepository = gatewayReadRepository;
    }

    public async Task ReadAllGatewaysAsync(CancellationToken ct)
    {
        var gateways = await _gatewayReadRepository.GetAllNoPaginateAsync(include:p=>p.Include(p=>p.Devices),cancellationToken:ct);
        //Where(d=>d.IsActive)
        _logger.LogInformation("Toplam {GatewayCount} gateway paralel olarak okunacak", gateways.Count);

        #region sınırsız paralel
        //// Tüm gateway'leri paralel olarak işle (sınırsız)
        //var gatewayTasks = gateways.Select(gateway => ProcessGatewayDevicesAsync(gateway, ct));
        //await Task.WhenAll(gatewayTasks);
        #endregion

        int batchSize = 50; // Maksimum 50 gateway paralel
        for (int i = 0; i < gateways.Count; i += batchSize)
        {
            var batch = gateways.Skip(i).Take(batchSize).ToList();

            var batchTasks = batch.Select(gateway => ProcessGatewayDevicesAsync(gateway, ct));

            await Task.WhenAll(batchTasks);

            _logger.LogInformation("Batch {BatchNumber} tamamlandı", (i / batchSize) + 1);
        }
    }

    private async Task ProcessGatewayDevicesAsync(Gateway gateway, CancellationToken ct)
    {
        _logger.LogInformation("Gateway {IpAddress}:{Port} cihazları okunuyor...",
            gateway.IpAddress, gateway.Port);

        // Gateway'deki tüm cihazları sırayla oku (RS485 seri yapısı için)
        foreach (var device in gateway.Devices)  // Where(d=>d.IsActive)
        {
            try
            {
                EnergyData? data = null;
                int attempt = 0;

                // Retry mekanizması
                while (attempt < _settings.RetryCount && data == null && !ct.IsCancellationRequested)
                {
                    attempt++;

                    _logger.LogDebug("Gateway {IpAddress} Device {UnitId} deneme {Attempt}",
                        gateway.IpAddress, device.UnitId, attempt);

                    data = await ReadDeviceDataAsync(gateway.IpAddress, gateway.Port, device.UnitId, device, ct);

                    if (data is null)
                    {
                        _logger.LogWarning("Device {UnitId} okunamadı, {Delay}ms sonra retry",
                            device.UnitId, _settings.InterRequestDelayMs);

                        await Task.Delay(_settings.InterRequestDelayMs, ct);
                    }
                }

                if (data != null)
                {
                    await SaveEnergyDataAsync(data, ct);
                    _logger.LogInformation("Device {UnitId} verisi başarıyla kaydedildi", device.UnitId);
                }
                else
                {
                    _logger.LogError("Device {UnitId} {RetryCount} denemede okunamadı",
                        device.UnitId, _settings.RetryCount);
                }

                // Cihazlar arası RS485 için bekleme
                if (!ct.IsCancellationRequested)
                {
                    await Task.Delay(_settings.InterDeviceDelayMs, ct);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Device {UnitId} işlenirken hata", device.UnitId);
            }
        }
    }

    private async Task<EnergyData?> ReadDeviceDataAsync(string ip, int port, int unitId, Device device, CancellationToken ct)
    {
        try
        {
            using var client = new TcpClient();

            var connectTask = client.ConnectAsync(ip, port);
            if (await Task.WhenAny(connectTask, Task.Delay(_settings.TimeoutMs, ct)) != connectTask)
            {
                _logger.LogWarning("Device {UnitId} TCP bağlantı timeout", unitId);
                return null; // Timeout -> atla
            }

            await connectTask;

            using var stream = client.GetStream();

            byte[] request = {
            0x00, 0x01,
            0x00, 0x00,
            0x00, 0x06,
            (byte)unitId,
            0x03,
            0x00, 0x00,
            0x00, 0x0E
        };

            await stream.WriteAsync(request, 0, request.Length, ct);

            byte[] buffer = new byte[256];

            // Cihazdan yanıt beklerken timeout
            var readTask = stream.ReadAsync(buffer, 0, buffer.Length, ct);
            if (await Task.WhenAny(readTask, Task.Delay(_settings.TimeoutMs, ct)) != readTask)
            {
                _logger.LogWarning("Device {UnitId} okuma timeout, atlanıyor", unitId);
                return null; // Timeout -> atla
            }

            int bytesRead = await readTask;
            _logger.LogDebug("Device {UnitId} response: {BytesRead} bytes", unitId, bytesRead);

            if (bytesRead >= 31)
            {
                return ParseModbusData(buffer, bytesRead, device.Id);
            }

            _logger.LogWarning("Device {UnitId} geçersiz response: {BytesRead} bytes", unitId, bytesRead);
            return null;
        }
        catch (SocketException sex)
        {
            _logger.LogWarning("Device {UnitId} network hatası: {Message}", unitId, sex.Message);
            return null; // Ağ hatası -> atla
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Device {UnitId} okuma hatası", unitId);
            return null; // Hata -> atla
        }
    }


    private EnergyData ParseModbusData(byte[] buffer, int bytesRead, Guid deviceId)
    {
        int offset = 9; // Modbus TCP response header sonrası

        return new EnergyData
        {
            DeviceId = deviceId,
            Timestamp = DateTime.UtcNow,
            Voltage = (buffer[offset++] << 8 | buffer[offset++]) / 100f,
            Current = (buffer[offset++] << 8 | buffer[offset++]) / 100f,
            Power = (buffer[offset++] << 8 | buffer[offset++]) / 10f,
            Energy = (uint)((buffer[offset++] << 24) | (buffer[offset++] << 16) | (buffer[offset++] << 8) | buffer[offset++]),
            Frequency = (buffer[offset++] << 8 | buffer[offset++]) / 100f,
            PowerFactor = (buffer[offset++] << 8 | buffer[offset++]) / 10000f,
            Status = buffer[offset++],
            LoadPercentage = buffer[offset]
        };
    }

    private async Task SaveEnergyDataAsync(EnergyData data, CancellationToken ct)
    {
        try
        {
            using var scope = _services.CreateScope();
            var _energyData = scope.ServiceProvider.GetRequiredService<IEnergyDataWriteRepository>();
            await _energyData.AddAsync(data, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EnergyData kaydedilirken hata");
        }
    }
}
