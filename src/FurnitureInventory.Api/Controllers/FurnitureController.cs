using Microsoft.AspNetCore.Mvc;
using FurnitureInventory.Core.Entities;
using FurnitureInventory.Core.Interfaces;
using FurnitureInventory.Api.Models;

namespace FurnitureInventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FurnitureController : ControllerBase
{
    private readonly IFurnitureService _furnitureService;
    private readonly ILogger<FurnitureController> _logger;

    public FurnitureController(IFurnitureService furnitureService, ILogger<FurnitureController> logger)
    {
        _furnitureService = furnitureService;
        _logger = logger;
    }

    /// <summary>
    /// Récupère tous les meubles
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FurnitureWithLocationDto>>> GetAll(CancellationToken cancellationToken)
    {
        var furnitures = await _furnitureService.GetAllAsync(cancellationToken);
        var dtos = furnitures.Select(f => f.ToWithLocationDto());
        return Ok(dtos);
    }

    /// <summary>
    /// Récupère un meuble par son ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<FurnitureWithLocationDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var furniture = await _furnitureService.GetByIdAsync(id, cancellationToken);
        if (furniture == null)
            return NotFound(new ApiErrorResponse 
            { 
                ErrorCode = ErrorCodes.FURNITURE_NOT_FOUND,
                Message = "Furniture not found"
            });

        return Ok(furniture.ToWithLocationDto());
    }

    /// <summary>
    /// Recherche des meubles par code barre
    /// </summary>
    [HttpGet("barcode/{barcode}")]
    public async Task<ActionResult<FurnitureWithLocationDto>> GetByBarcode(string barcode, CancellationToken cancellationToken)
    {
        var furniture = await _furnitureService.GetByBarcodeAsync(barcode, cancellationToken);
        if (furniture == null)
            return NotFound(new ApiErrorResponse 
            { 
                ErrorCode = ErrorCodes.FURNITURE_NOT_FOUND,
                Message = "Furniture not found"
            });

        return Ok(furniture.ToWithLocationDto());
    }

    /// <summary>
    /// Recherche des meubles avec des critères
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<FurnitureWithLocationDto>>> Search(
        [FromQuery] string? reference,
        [FromQuery] string? famille,
        [FromQuery] string? site,
        CancellationToken cancellationToken)
    {
        var furnitures = await _furnitureService.SearchAsync(reference, famille, site, cancellationToken);
        var dtos = furnitures.Select(f => f.ToWithLocationDto());
        return Ok(dtos);
    }

    /// <summary>
    /// Crée un nouveau meuble
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<FurnitureWithLocationDto>> Create([FromBody] FurnitureDto furnitureDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiErrorResponse 
            { 
                ErrorCode = ErrorCodes.VALIDATION_ERROR,
                Message = "Model validation failed",
                Details = ModelState
            });

        var furniture = furnitureDto.ToEntity();
        var created = await _furnitureService.CreateAsync(furniture, cancellationToken);
        var createdDto = created.ToWithLocationDto();
        return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
    }

    /// <summary>
    /// Met à jour un meuble
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<FurnitureWithLocationDto>> Update(int id, [FromBody] FurnitureDto furnitureDto, CancellationToken cancellationToken)
    {
        if (id != furnitureDto.Id)
            return BadRequest(new ApiErrorResponse 
            { 
                ErrorCode = ErrorCodes.ID_MISMATCH,
                Message = "ID mismatch"
            });

        if (!ModelState.IsValid)
            return BadRequest(new ApiErrorResponse 
            { 
                ErrorCode = ErrorCodes.VALIDATION_ERROR,
                Message = "Model validation failed",
                Details = ModelState
            });

        var furniture = furnitureDto.ToEntity();
        var updated = await _furnitureService.UpdateAsync(furniture, cancellationToken);
        return Ok(updated.ToWithLocationDto());
    }

    /// <summary>
    /// Supprime un meuble
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _furnitureService.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound(new ApiErrorResponse 
            { 
                ErrorCode = ErrorCodes.FURNITURE_NOT_FOUND,
                Message = "Furniture not found"
            });

        return NoContent();
    }

    /// <summary>
    /// Assigne une localisation à un meuble
    /// </summary>
    [HttpPost("{id}/location/{locationId}")]
    public async Task<ActionResult> AssignLocation(int id, int locationId, CancellationToken cancellationToken)
    {
        var result = await _furnitureService.AssignLocationAsync(id, locationId, cancellationToken);
        if (!result)
            return NotFound(new ApiErrorResponse 
            { 
                ErrorCode = ErrorCodes.OPERATION_FAILED,
                Message = "Operation failed - furniture or location not found"
            });

        return NoContent();
    }

    /// <summary>
    /// Assigne un tag RFID à un meuble
    /// </summary>
    [HttpPost("{id}/rfid/{rfidTagId}")]
    public async Task<ActionResult> AssignRfidTag(int id, int rfidTagId, CancellationToken cancellationToken)
    {
        var result = await _furnitureService.AssignRfidTagAsync(id, rfidTagId, cancellationToken);
        if (!result)
            return NotFound(new ApiErrorResponse 
            { 
                ErrorCode = ErrorCodes.OPERATION_FAILED,
                Message = "Operation failed - furniture or RFID tag not found"
            });

        return NoContent();
    }

    /// <summary>
    /// Récupère la position x,y d'un meuble
    /// </summary>
    [HttpGet("{id}/position")]
    public async Task<ActionResult<object>> GetPosition(int id, CancellationToken cancellationToken)
    {
        var position = await _furnitureService.GetPositionAsync(id, cancellationToken);
        if (position == null)
            return NotFound(new ApiErrorResponse 
            { 
                ErrorCode = ErrorCodes.FURNITURE_NOT_FOUND,
                Message = "Furniture not found"
            });

        return Ok(new { x = position.Value.x, y = position.Value.y });
    }
}
