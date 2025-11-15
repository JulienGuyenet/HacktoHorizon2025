namespace FurnitureInventory.Api.Models;

/// <summary>
/// DTO pour une localisation avec la liste des meubles
/// </summary>
public class LocationWithFurnitureDto
{
    public int Id { get; set; }
    public string BuildingName { get; set; } = string.Empty;
    public string? Floor { get; set; }
    public string? Room { get; set; }
    public string? Zone { get; set; }
    public string? Description { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Liste des meubles à cette localisation (sans référence circulaire à la localisation)
    /// </summary>
    public ICollection<FurnitureDto> Furnitures { get; set; } = new List<FurnitureDto>();
}
