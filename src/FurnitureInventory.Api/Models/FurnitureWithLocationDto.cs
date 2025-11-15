namespace FurnitureInventory.Api.Models;

/// <summary>
/// DTO pour un meuble avec les détails de sa localisation
/// </summary>
public class FurnitureWithLocationDto
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
    public int? LocationId { get; set; }
    public int? RfidTagId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Détails de la localisation (sans référence circulaire aux meubles)
    /// </summary>
    public LocationDto? Location { get; set; }
}
