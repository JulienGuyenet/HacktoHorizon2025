using Microsoft.AspNetCore.Mvc;
using FurnitureInventory.Core.Entities;
using FurnitureInventory.Core.Interfaces;
using FurnitureInventory.Api.Models;

namespace FurnitureInventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;
    private readonly ILogger<LocationController> _logger;

    public LocationController(ILocationService locationService, ILogger<LocationController> logger)
    {
        _locationService = locationService;
        _logger = logger;
    }

    /// <summary>
    /// Récupère toutes les localisations
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationDto>>> GetAll(CancellationToken cancellationToken)
    {
        var locations = await _locationService.GetAllAsync(cancellationToken);
        var dtos = locations.Select(l => l.ToDto());
        return Ok(dtos);
    }

    /// <summary>
    /// Récupère une localisation par son ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<LocationDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var location = await _locationService.GetByIdAsync(id, cancellationToken);
        if (location == null)
            return NotFound();

        return Ok(location.ToDto());
    }

    /// <summary>
    /// Récupère une localisation avec tous ses meubles
    /// </summary>
    [HttpGet("{id}/furniture")]
    public async Task<ActionResult<IEnumerable<FurnitureDto>>> GetFurnitureAtLocation(int id, CancellationToken cancellationToken)
    {
        var furnitures = await _locationService.GetFurnitureAtLocationAsync(id, cancellationToken);
        var dtos = furnitures.Select(f => f.ToDto());
        return Ok(dtos);
    }

    /// <summary>
    /// Recherche des localisations par bâtiment
    /// </summary>
    [HttpGet("building/{buildingName}")]
    public async Task<ActionResult<IEnumerable<LocationDto>>> GetByBuilding(string buildingName, CancellationToken cancellationToken)
    {
        var locations = await _locationService.GetByBuildingAsync(buildingName, cancellationToken);
        var dtos = locations.Select(l => l.ToDto());
        return Ok(dtos);
    }

    /// <summary>
    /// Crée une nouvelle localisation
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<LocationDto>> Create([FromBody] LocationDto locationDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var location = locationDto.ToEntity();
        var created = await _locationService.CreateAsync(location, cancellationToken);
        var createdDto = created.ToDto();
        return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
    }

    /// <summary>
    /// Met à jour une localisation
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<LocationDto>> Update(int id, [FromBody] LocationDto locationDto, CancellationToken cancellationToken)
    {
        if (id != locationDto.Id)
            return BadRequest("ID mismatch");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var location = locationDto.ToEntity();
        var updated = await _locationService.UpdateAsync(location, cancellationToken);
        return Ok(updated.ToDto());
    }

    /// <summary>
    /// Supprime une localisation
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _locationService.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
