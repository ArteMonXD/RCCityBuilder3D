using Domain.Enums;
using Domain.Models;
using Presentation.Gameplay.Presenters;
using R3;
using System.Collections.Generic;
using UnityEngine;

namespace Presentation.Gameplay.Views
{
    public class GridView : MonoBehaviour, IGridView
    {
        [SerializeField] private GameObject gridCellPrefab;
        [SerializeField] private GameObject buildingPrefab;
        [SerializeField] private int gridSize = 32;

        public Subject<GridPosition> OnCellClicked { get; } = new();

        private Dictionary<GridPosition, GameObject> _gridCells = new();
        private Dictionary<int, GameObject> _buildings = new();
        private SelectionMode _currentSelectionMode;

        private void Start()
        {
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    var position = new GridPosition(x, z);
                    var cell = Instantiate(gridCellPrefab, position.ToWorldPosition(), Quaternion.identity, transform);
                    _gridCells[position] = cell;

                    // Setup click handler
                    var trigger = cell.GetComponent<GridCellTrigger>();
                    if (trigger != null)
                    {
                        trigger.Initialize(position, OnCellClicked);
                    }
                }
            }
        }

        public void ShowBuilding(BuildingModel building)
        {
            var buildingObj = Instantiate(buildingPrefab, building.Position.ToWorldPosition(), Quaternion.identity);
            _buildings[building.Id] = buildingObj;

            // Setup building visual based on type and level
            var renderer = buildingObj.GetComponent<Renderer>();
            renderer.material.color = GetBuildingColor(building.Type);
        }

        public void ClearHighlights()
        {
            foreach (var cell in _gridCells.Values)
            {
                cell.GetComponent<Renderer>().material.color = Color.white;
            }
        }

        public void ShowPlacementError(GridPosition position, string reason)
        {
            if (_gridCells.TryGetValue(position, out var cell))
            {
                cell.GetComponent<Renderer>().material.color = Color.red;
            }
        }

        public void SetSelectionMode(SelectionMode mode)
        {
            _currentSelectionMode = mode;
            ClearHighlights();
        }

        private Color GetBuildingColor(BuildingType type) => type switch
        {
            BuildingType.House => Color.blue,
            BuildingType.Farm => Color.green,
            BuildingType.Mine => Color.gray,
            _ => Color.white
        };
    }

    public interface IGridView
    {
        Subject<GridPosition> OnCellClicked { get; }
        void ShowBuilding(BuildingModel building);
        void ClearHighlights();
        void ShowPlacementError(GridPosition position, string reason);
        void SetSelectionMode(SelectionMode mode);
    }
}
