namespace FurnitureInventory.Api.Dtos;

/// <summary>
/// DTO pour représenter une localisation sans dépendance cyclique
/// </summary>
public class LocationDto
{
    public int Id { get; set; }
    public string BuildingName { get; set; } = string.Empty;
    public string? Floor { get; set; }
    public string? Room { get; set; }
    public string? Zone { get; set; }
    public string? Description { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    
    // Relations
    public ICollection<FurnitureSummaryDto> Furnitures { get; set; } = new List<FurnitureSummaryDto>();
    
    // Métadonnées
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
