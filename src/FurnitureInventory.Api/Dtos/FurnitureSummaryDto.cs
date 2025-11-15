namespace FurnitureInventory.Api.Dtos;

/// <summary>
/// Résumé simplifié d'un meuble (pour éviter les dépendances cycliques)
/// </summary>
public class FurnitureSummaryDto
{
    public int Id { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string? Famille { get; set; }
    public string? Type { get; set; }
    public string? CodeBarre { get; set; }
}
