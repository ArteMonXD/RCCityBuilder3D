using System;
using Domain.Enums;

namespace Domain.Models
{
    public class BuildingModel
    {
        public int Id { get; }
        public BuildingType Type { get; }
        public int Level { get; private set; } = 1;
        public GridPosition Position { get; private set; }
        public BuildingProcessStatus ProcessStatus { get; private set; }
            = BuildingProcessStatus.None;

        // Событие для уведомления об изменениях
        public event Action<BuildingProcessStatus> ProcessStatusChanged;

        public BuildingModel(int id, BuildingType type, GridPosition position)
        {
            Id = id;
            Type = type;
            Position = position;
        }

        public void Upgrade()
        {
            Level++;
            // Можно добавить логику изменения статуса при апгрейде
        }

        public void MoveTo(GridPosition newPosition) => Position = newPosition;

        public void SetProcessStatus(BuildingProcessStatus status)
        {
            if (ProcessStatus != status)
            {
                ProcessStatus = status;
                ProcessStatusChanged?.Invoke(status);
            }
        }

        public int GetIncome() => Type switch
        {
            BuildingType.House => Level * 5,
            BuildingType.Farm => Level * 10,
            BuildingType.Mine => Level * 15,
            _ => 0
        };

        public int GetUpgradeCost() => Type switch
        {
            BuildingType.House => Level * 50,
            BuildingType.Farm => Level * 75,
            BuildingType.Mine => Level * 100,
            _ => 0
        };
    }
}
