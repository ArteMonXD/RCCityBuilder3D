using System;
using System.Numerics;
using Domain.Enums;
using Domain.Models;


namespace Domain.MessagesDTO
{
    // Building Operations
    [Serializable]
    public class BuildingPlaceRequestDTO
    {
        public BuildingType BuildingType;
        public GridPosition Position;

        public BuildingPlaceRequestDTO() { }

        public BuildingPlaceRequestDTO(BuildingType buildingType, GridPosition position)
        {
            BuildingType = buildingType;
            Position = position;
        }
    }

    [Serializable]
    public class BuildingPlacedDTO
    {
        public BuildingModel Building;

        public BuildingPlacedDTO() { }

        public BuildingPlacedDTO(BuildingModel building)
        {
            Building = building;
        }
    }

    [Serializable]
    public class BuildingPlacementFailedDTO
    {
        public string Reason;
        public GridPosition Position;

        public BuildingPlacementFailedDTO() { }

        public BuildingPlacementFailedDTO(string reason, GridPosition position)
        {
            Reason = reason;
            Position = position;
        }
    }

    [Serializable]
    public class UpgradeBuildingRequestDTO
    {
        public int BuildingId;

        public UpgradeBuildingRequestDTO() { }

        public UpgradeBuildingRequestDTO(int buildingId)
        {
            BuildingId = buildingId;
        }
    }

    [Serializable]
    public class BuildingUpgradedDTO
    {
        public int BuildingId;
        public int NewLevel;

        public BuildingUpgradedDTO() { }

        public BuildingUpgradedDTO(int buildingId, int newLevel)
        {
            BuildingId = buildingId;
            NewLevel = newLevel;
        }
    }

    [Serializable]
    public class UpgradeBuildingFailedDTO
    {
        public int BuildingId;
        public string Reason;

        public UpgradeBuildingFailedDTO() { }

        public UpgradeBuildingFailedDTO(int buildingId, string reason)
        {
            BuildingId = buildingId;
            Reason = reason;
        }
    }

    [Serializable]
    public class MoveBuildingRequestDTO
    {
        public int BuildingId;
        public GridPosition NewPosition;

        public MoveBuildingRequestDTO() { }

        public MoveBuildingRequestDTO(int buildingId, GridPosition newPosition)
        {
            BuildingId = buildingId;
            NewPosition = newPosition;
        }
    }

    [Serializable]
    public class BuildingMovedDTO
    {
        public int BuildingId;
        public GridPosition OldPosition;
        public GridPosition NewPosition;

        public BuildingMovedDTO() { }

        public BuildingMovedDTO(int buildingId, GridPosition oldPosition, GridPosition newPosition)
        {
            BuildingId = buildingId;
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }
    }

    [Serializable]
    public class MoveBuildingFailedDTO
    {
        public int BuildingId;
        public string Reason;

        public MoveBuildingFailedDTO() { }

        public MoveBuildingFailedDTO(int buildingId, string reason)
        {
            BuildingId = buildingId;
            Reason = reason;
        }
    }

    [Serializable]
    public class RemoveBuildingRequestDTO
    {
        public int BuildingId;

        public RemoveBuildingRequestDTO() { }

        public RemoveBuildingRequestDTO(int buildingId)
        {
            BuildingId = buildingId;
        }
    }

    [Serializable]
    public class BuildingRemovedDTO
    {
        public int BuildingId;

        public BuildingRemovedDTO() { }

        public BuildingRemovedDTO(int buildingId)
        {
            BuildingId = buildingId;
        }
    }

    // Economy
    [Serializable]
    public class ResourceBalanceChangedDTO
    {
        public int Gold;
        public int Change;

        public ResourceBalanceChangedDTO() { }

        public ResourceBalanceChangedDTO(int gold, int change)
        {
            Gold = gold;
            Change = change;
        }
    }

    [Serializable]
    public class PassiveIncomeDTO
    {
        public int Amount;

        public PassiveIncomeDTO() { }

        public PassiveIncomeDTO(int amount)
        {
            Amount = amount;
        }
    }

    // Input
    [Serializable]
    public class CameraMoveDTO
    {
        public Vector2 Direction;

        public CameraMoveDTO() { }

        public CameraMoveDTO(Vector2 direction)
        {
            Direction = direction;
        }
    }

    [Serializable]
    public class CameraZoomDTO
    {
        public float ZoomDelta;

        public CameraZoomDTO() { }

        public CameraZoomDTO(float zoomDelta)
        {
            ZoomDelta = zoomDelta;
        }
    }

    [Serializable]
    public class BuildingSelectedDTO
    {
        public int BuildingId;

        public BuildingSelectedDTO() { }

        public BuildingSelectedDTO(int buildingId)
        {
            BuildingId = buildingId;
        }
    }

    // Save/Load
    [Serializable]
    public class SaveGameRequestDTO
    {
        public string SaveSlot = "autosave";

        public SaveGameRequestDTO() { }

        public SaveGameRequestDTO(string saveSlot)
        {
            SaveSlot = saveSlot;
        }
    }

    [Serializable]
    public class GameSavedDTO
    {
        public string SaveSlot;
        public DateTime Timestamp;

        public GameSavedDTO() { }

        public GameSavedDTO(string saveSlot, DateTime timestamp)
        {
            SaveSlot = saveSlot;
            Timestamp = timestamp;
        }
    }

    [Serializable]
    public class LoadGameRequestDTO
    {
        public string SaveSlot;

        public LoadGameRequestDTO() { }

        public LoadGameRequestDTO(string saveSlot)
        {
            SaveSlot = saveSlot;
        }
    }

    [Serializable]
    public class GameLoadedDTO
    {
        public GameStateModel GameState;

        public GameLoadedDTO() { }

        public GameLoadedDTO(GameStateModel gameState)
        {
            GameState = gameState;
        }
    }
}