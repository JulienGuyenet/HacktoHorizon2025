using FurnitureInventory.Core.Entities;

namespace FurnitureInventory.Api.Dtos;

/// <summary>
/// Méthodes d'extension pour mapper les entités vers les DTOs
/// </summary>
public static class DtoMappingExtensions
{
    /// <summary>
    /// Convertit un Furniture en FurnitureDto
    /// </summary>
    public static FurnitureDto ToDto(this Furniture furniture)
    {
        return new FurnitureDto
        {
            Id = furniture.Id,
            Reference = furniture.Reference,
            Designation = furniture.Designation,
            Famille = furniture.Famille,
            Type = furniture.Type,
            Fournisseur = furniture.Fournisseur,
            Utilisateur = furniture.Utilisateur,
            CodeBarre = furniture.CodeBarre,
            NumeroSerie = furniture.NumeroSerie,
            Informations = furniture.Informations,
            Site = furniture.Site,
            DateLivraison = furniture.DateLivraison,
            PositionX = furniture.PositionX,
            PositionY = furniture.PositionY,
            LocationId = furniture.LocationId,
            Location = furniture.Location?.ToSummaryDto(),
            RfidTagId = furniture.RfidTagId,
            CreatedAt = furniture.CreatedAt,
            UpdatedAt = furniture.UpdatedAt
        };
    }

    /// <summary>
    /// Convertit un Furniture en FurnitureSummaryDto
    /// </summary>
    public static FurnitureSummaryDto ToSummaryDto(this Furniture furniture)
    {
        return new FurnitureSummaryDto
        {
            Id = furniture.Id,
            Reference = furniture.Reference,
            Designation = furniture.Designation,
            Famille = furniture.Famille,
            Type = furniture.Type,
            CodeBarre = furniture.CodeBarre
        };
    }

    /// <summary>
    /// Convertit une Location en LocationDto
    /// </summary>
    public static LocationDto ToDto(this Location location)
    {
        return new LocationDto
        {
            Id = location.Id,
            BuildingName = location.BuildingName,
            Floor = location.Floor,
            Room = location.Room,
            Zone = location.Zone,
            Description = location.Description,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            Furnitures = location.Furnitures.Select(f => f.ToSummaryDto()).ToList(),
            CreatedAt = location.CreatedAt,
            UpdatedAt = location.UpdatedAt
        };
    }

    /// <summary>
    /// Convertit une Location en LocationSummaryDto
    /// </summary>
    public static LocationSummaryDto ToSummaryDto(this Location location)
    {
        return new LocationSummaryDto
        {
            Id = location.Id,
            BuildingName = location.BuildingName,
            Floor = location.Floor,
            Room = location.Room,
            Zone = location.Zone,
            Description = location.Description
        };
    }

    /// <summary>
    /// Convertit une collection de Furniture en FurnitureDto
    /// </summary>
    public static IEnumerable<FurnitureDto> ToDtos(this IEnumerable<Furniture> furnitures)
    {
        return furnitures.Select(f => f.ToDto());
    }

    /// <summary>
    /// Convertit une collection de Location en LocationDto
    /// </summary>
    public static IEnumerable<LocationDto> ToDtos(this IEnumerable<Location> locations)
    {
        return locations.Select(l => l.ToDto());
    }
}
