namespace FurnitureInventory.Api.Dtos;

/// <summary>
/// DTO pour représenter un meuble sans dépendance cyclique
/// </summary>
public class FurnitureDto
{
    public int Id { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string? Famille { get; set; }
    public string? Type { get; set; }
    public string? Fournisseur { get; set; }
    public string? Utilisateur { get; set; }
    public string? CodeBarre { get; set; }
    public string? NumeroSerie { get; set; }
    public string? Informations { get; set; }
    public string? Site { get; set; }
    public DateTime? DateLivraison { get; set; }
    public double? PositionX { get; set; }
    public double? PositionY { get; set; }
    
    // Relations
    public int? LocationId { get; set; }
    public LocationSummaryDto? Location { get; set; }
    
    public int? RfidTagId { get; set; }
    
    // Métadonnées
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
