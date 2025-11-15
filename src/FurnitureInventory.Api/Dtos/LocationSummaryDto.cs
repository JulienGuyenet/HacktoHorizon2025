namespace FurnitureInventory.Api.Dtos;

/// <summary>
/// Résumé simplifié d'une localisation (pour éviter les dépendances cycliques)
/// </summary>
public class LocationSummaryDto
{
    public int Id { get; set; }
    public string BuildingName { get; set; } = string.Empty;
    public string? Floor { get; set; }
    public string? Room { get; set; }
    public string? Zone { get; set; }
    public string? Description { get; set; }
}
