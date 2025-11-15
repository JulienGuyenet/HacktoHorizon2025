namespace FurnitureInventory.Api.Models;

/// <summary>
/// DTO pour un meuble sans dépendances circulaires
/// </summary>
public class FurnitureDto
{
    public int Id { get; set; }
    
    /// <summary>
    /// Référence unique du meuble
    /// </summary>
    public string Reference { get; set; } = string.Empty;
    
    /// <summary>
    /// Désignation (nom/description) du meuble
    /// </summary>
    public string Designation { get; set; } = string.Empty;
    
    /// <summary>
    /// Famille du meuble
    /// </summary>
    public string? Famille { get; set; }
    
    /// <summary>
    /// Type de meuble
    /// </summary>
    public string? Type { get; set; }
    
    /// <summary>
    /// Fournisseur du meuble
    /// </summary>
    public string? Fournisseur { get; set; }
    
    /// <summary>
    /// Utilisateur actuel du meuble
    /// </summary>
    public string? Utilisateur { get; set; }
    
    /// <summary>
    /// Code barre pour identification
    /// </summary>
    public string? CodeBarre { get; set; }
    
    /// <summary>
    /// Numéro de série du meuble
    /// </summary>
    public string? NumeroSerie { get; set; }
    
    /// <summary>
    /// Informations complémentaires
    /// </summary>
    public string? Informations { get; set; }
    
    /// <summary>
    /// Site où se trouve le meuble
    /// </summary>
    public string? Site { get; set; }
    
    /// <summary>
    /// Date de livraison du meuble
    /// </summary>
    public DateTime? DateLivraison { get; set; }
    
    /// <summary>
    /// Position X du meuble dans le plan d'étage
    /// </summary>
    public double? PositionX { get; set; }
    
    /// <summary>
    /// Position Y du meuble dans le plan d'étage
    /// </summary>
    public double? PositionY { get; set; }
    
    /// <summary>
    /// ID de la localisation actuelle
    /// </summary>
    public int? LocationId { get; set; }
    
    /// <summary>
    /// ID du tag RFID associé
    /// </summary>
    public int? RfidTagId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
