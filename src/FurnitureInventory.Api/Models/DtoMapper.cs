using FurnitureInventory.Core.Entities;

namespace FurnitureInventory.Api.Models;

/// <summary>
/// Classe utilitaire pour convertir les entités en DTOs
/// </summary>
public static class DtoMapper
{
    /// <summary>
    /// Convertit une entité Location en LocationDto
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
            CreatedAt = location.CreatedAt,
            UpdatedAt = location.UpdatedAt
        };
    }

    /// <summary>
    /// Convertit une entité Furniture en FurnitureDto
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
            RfidTagId = furniture.RfidTagId,
            CreatedAt = furniture.CreatedAt,
            UpdatedAt = furniture.UpdatedAt
        };
    }

    /// <summary>
    /// Convertit une entité Furniture avec Location en FurnitureWithLocationDto
    /// </summary>
    public static FurnitureWithLocationDto ToWithLocationDto(this Furniture furniture)
    {
        return new FurnitureWithLocationDto
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
            RfidTagId = furniture.RfidTagId,
            CreatedAt = furniture.CreatedAt,
            UpdatedAt = furniture.UpdatedAt,
            Location = furniture.Location?.ToDto()
        };
    }

    /// <summary>
    /// Convertit une entité Location avec Furnitures en LocationWithFurnitureDto
    /// </summary>
    public static LocationWithFurnitureDto ToWithFurnitureDto(this Location location)
    {
        return new LocationWithFurnitureDto
        {
            Id = location.Id,
            BuildingName = location.BuildingName,
            Floor = location.Floor,
            Room = location.Room,
            Zone = location.Zone,
            Description = location.Description,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            CreatedAt = location.CreatedAt,
            UpdatedAt = location.UpdatedAt,
            Furnitures = location.Furnitures?.Select(f => f.ToDto()).ToList() ?? new List<FurnitureDto>()
        };
    }

    /// <summary>
    /// Convertit un LocationDto en entité Location pour la création/mise à jour
    /// </summary>
    public static Location ToEntity(this LocationDto dto)
    {
        return new Location
        {
            Id = dto.Id,
            BuildingName = dto.BuildingName,
            Floor = dto.Floor,
            Room = dto.Room,
            Zone = dto.Zone,
            Description = dto.Description,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    /// <summary>
    /// Convertit un FurnitureDto en entité Furniture pour la création/mise à jour
    /// </summary>
    public static Furniture ToEntity(this FurnitureDto dto)
    {
        return new Furniture
        {
            Id = dto.Id,
            Reference = dto.Reference,
            Designation = dto.Designation,
            Famille = dto.Famille,
            Type = dto.Type,
            Fournisseur = dto.Fournisseur,
            Utilisateur = dto.Utilisateur,
            CodeBarre = dto.CodeBarre,
            NumeroSerie = dto.NumeroSerie,
            Informations = dto.Informations,
            Site = dto.Site,
            DateLivraison = dto.DateLivraison,
            PositionX = dto.PositionX,
            PositionY = dto.PositionY,
            LocationId = dto.LocationId,
            RfidTagId = dto.RfidTagId,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }
}
