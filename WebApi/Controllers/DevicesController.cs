using Application.Interfaces.RepositoryServices;
using Domain;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ServerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DevicesController : BaseController
{
    private readonly IDeviceReadRepository _deviceReadRepository;
    private readonly IDeviceWriteRepository _deviceWriteRepository;

    public DevicesController(IDeviceReadRepository deviceReadRepository, IDeviceWriteRepository deviceWriteRepository)
    {
        _deviceReadRepository = deviceReadRepository;
        _deviceWriteRepository = deviceWriteRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var devices = await _deviceReadRepository.Query()
            .Select(d => new DeviceReadDto
            {
                Id = d.Id,
                UnitId = d.UnitId,
                Name = d.Name,
                Location = d.Location
            }).ToListAsync();
        return Ok(devices);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var d = await _deviceReadRepository.GetAsync(p=>p.Id==id);
        if (d is null) return NotFound();

        var dto = new DeviceReadDto
        {
            Id = d.Id,
            UnitId = d.UnitId,
            Name = d.Name,
            Location = d.Location
        };
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DeviceCreateDto dto)
    {
        var device = new Device
        {
            UnitId = dto.UnitId,
            Name = dto.Name,
            Location = dto.Location,
            GatewayId = dto.GatewayId
        };
         await _deviceWriteRepository.AddAsync(device);

        return CreatedAtAction(nameof(Get), new { id = device.Id }, device);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] DeviceUpdateDto dto)
    {
        var existing = await _deviceReadRepository.GetAsync(p => p.Id == id);
        if (existing is null) return NotFound();

        existing.UnitId = dto.UnitId;
        existing.Name = dto.Name;
        existing.Location = dto.Location;
        existing.GatewayId = dto.GatewayId;
        
        await _deviceWriteRepository.UpdateAsync(existing);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _deviceReadRepository.GetAsync(p => p.Id == id);
        if (existing == null) return NotFound();
        await _deviceWriteRepository.DeleteAsync(existing);
        return NoContent();
    }
}
