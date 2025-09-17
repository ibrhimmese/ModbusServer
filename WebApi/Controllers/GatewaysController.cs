using Application.Interfaces.RepositoryServices;
using Domain;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ServerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GatewaysController : BaseController
{
    private readonly IGatewayReadRepository _gatewayReadRepository;
    private readonly IGatewayWriteRepository _gatewayWriteRepository;

    public GatewaysController(IGatewayReadRepository gatewayReadRepository, IGatewayWriteRepository gatewayWriteRepository)
    {
        _gatewayReadRepository = gatewayReadRepository;
        _gatewayWriteRepository = gatewayWriteRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var gateways = await _gatewayReadRepository.Query()
            .Include(g => g.Devices)
            .Select(g => new GatewayReadDto
            {
                Id = g.Id,
                IpAddress = g.IpAddress,
                Port = g.Port,
                Devices = g.Devices.Select(d => new DeviceReadDto
                {
                    Id = d.Id,
                    UnitId = d.UnitId,
                    Name = d.Name,
                    Location = d.Location
                }).ToList()
            }).ToListAsync();
        return Ok(gateways);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var g = await _gatewayReadRepository.GetAsync(p=>p.Id==id,include:p=>p.Include(gw => gw.Devices));
        if (g is null) return NotFound();

        var dto = new GatewayReadDto
        {
            Id = g.Id,
            IpAddress = g.IpAddress,
            Port = g.Port,
            Devices = g.Devices.Select(d => new DeviceReadDto
            {
                Id = d.Id,
                UnitId = d.UnitId,
                Name = d.Name,
                Location = d.Location
            }).ToList()
        };
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] GatewayCreateDto dto)
    {
        var gateway = new Gateway
        {
            IpAddress = dto.IpAddress,
            Port = dto.Port
        };
        await _gatewayWriteRepository.AddAsync(gateway);
        return CreatedAtAction(nameof(Get), new { id = gateway.Id }, gateway);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] GatewayUpdateDto dto)
    {
        var existing = await _gatewayReadRepository.GetAsync(p => p.Id == id);
        if (existing is null) return NotFound();

        existing.IpAddress = dto.IpAddress;
        existing.Port = dto.Port;
        
        await _gatewayWriteRepository.UpdateAsync(existing);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _gatewayReadRepository.GetAsync(p => p.Id == id);
        if (existing is null) return NotFound();
       
        await _gatewayWriteRepository.DeleteAsync(existing);
        return NoContent();
    }
}
