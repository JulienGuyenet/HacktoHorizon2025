namespace FurnitureInventory.Api.Models;

/// <summary>
/// DTO pour une localisation sans dépendances circulaires
/// </summary>
public class LocationDto
{
    public int Id { get; set; }
    
    /// <summary>
    /// Nom du bâtiment
    /// </summary>
    public string BuildingName { get; set; } = string.Empty;
    
    /// <summary>
    /// Numéro ou nom de l'étage
    /// </summary>
    public string? Floor { get; set; }
    
    /// <summary>
    /// Numéro ou nom de la salle/pièce
    /// </summary>
    public string? Room { get; set; }
    
    /// <summary>
    /// Zone spécifique dans la pièce
    /// </summary>
    public string? Zone { get; set; }
    
    /// <summary>
    /// Description complète de la localisation
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Coordonnées GPS - Latitude
    /// </summary>
    public double? Latitude { get; set; }
    
    /// <summary>
    /// Coordonnées GPS - Longitude
    /// </summary>
    public double? Longitude { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
