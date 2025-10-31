using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Models
{
    public class GameStateModel
    {
        public int Gold { get; set; } = 1000;
        public Dictionary<GridPosition, BuildingModel> Buildings { get; set; } = new();
        public Dictionary<BuildingType, int> BuildingCosts { get; } = new()
        {
            [BuildingType.House] = 100,
            [BuildingType.Farm] = 150,
            [BuildingType.Mine] = 200
        };
    }
}
