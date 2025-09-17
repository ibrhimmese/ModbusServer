using Application.Interfaces.RepositoryServices;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ServerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EnergyDataController : BaseController
{
    private readonly IEnergyDataReadRepository _energyDataReadRepository;

    public EnergyDataController(IEnergyDataReadRepository energyDataReadRepository)
    {
        _energyDataReadRepository = energyDataReadRepository;
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatest()
    {
        var data = await _energyDataReadRepository.Query()
            .Join(
                _energyDataReadRepository.Query()
                    .GroupBy(e => e.DeviceId)
                    .Select(g => new
                    {
                        DeviceId = g.Key,
                        LastTimestamp = g.Max(x => x.Timestamp)
                    }),
                e => new { e.DeviceId, e.Timestamp },
                l => new { l.DeviceId, Timestamp = l.LastTimestamp },
                (e, l) => e
            )
            .Select(e => new EnergyDataReadDto
            {
                Id = e.Id,
                DeviceId = e.DeviceId,
                DeviceName = e.Device.Name,
                GatewayIp = e.Device.Gateway.IpAddress,
                Timestamp = e.Timestamp,
                Voltage = e.Voltage,
                Current = e.Current,
                Power = e.Power,
                Energy = e.Energy,
                Frequency = e.Frequency,
                PowerFactor = e.PowerFactor,
                Status = e.Status,
                LoadPercentage = e.LoadPercentage
            })
            .ToListAsync();

        return Ok(data);
    }




    [HttpGet("range-closest")]
    public async Task<IActionResult> GetRangeClosest(DateTime? start = null, DateTime? end = null)
    {
        // Eğer parametre verilmemişse default tarihleri belirleyelim
        var endDate = end ?? DateTime.UtcNow;
        var startDate = start ?? endDate.AddMonths(-1); // Son 1 ay

        var results = await _energyDataReadRepository.Query()
           .Where(e => e.Timestamp >= startDate && e.Timestamp <= endDate)
           .GroupBy(e => e.DeviceId)
           .Select(g => new
           {
               DeviceId = g.Key,
               DeviceName = g.Select(x => x.Device.Name).FirstOrDefault(),
               GatewayIp = g.Select(x => x.Device.Gateway.IpAddress).FirstOrDefault(),
               Start = g.OrderBy(x => x.Timestamp).Select(x => new
               {
                   x.Timestamp,
                   x.Voltage,
                   x.Current,
                   x.Power,
                   x.Energy,
                   x.Frequency,
                   x.PowerFactor,
                   x.Status,
                   x.LoadPercentage
               }).FirstOrDefault(),
               End = g.OrderByDescending(x => x.Timestamp).Select(x => new
               {
                   x.Timestamp,
                   x.Voltage,
                   x.Current,
                   x.Power,
                   x.Energy,
                   x.Frequency,
                   x.PowerFactor,
                   x.Status,
                   x.LoadPercentage
               }).FirstOrDefault()
           })
           .ToListAsync();

        return Ok(results);
    }



    // Cihaz bazlı detaylı okuma
    [HttpGet("device/{deviceId}")]
    public async Task<IActionResult> GetByDevice(Guid deviceId)
    {
        var data = await _energyDataReadRepository.Query()
            .Include(e => e.Device)
            .ThenInclude(d => d.Gateway)
            .Where(e => e.DeviceId == deviceId)
            .OrderBy(e => e.Timestamp)
            .Select(e => new EnergyDataReadDto
            {
                Id = e.Id,
                DeviceId = e.DeviceId,
                DeviceName = e.Device.Name,
                GatewayIp = e.Device.Gateway.IpAddress,
                Timestamp = e.Timestamp,
                Voltage = e.Voltage,
                Current = e.Current,
                Power = e.Power,
                Energy = e.Energy,
                Frequency = e.Frequency,
                PowerFactor = e.PowerFactor,
                Status = e.Status,
                LoadPercentage = e.LoadPercentage
            })
            .ToListAsync();

        return Ok(data);
    }
}
