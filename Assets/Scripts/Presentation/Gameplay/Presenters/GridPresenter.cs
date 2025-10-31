using Domain.Models;
using Domain.MessagesDTO;
using Domain.Enums;
using MessagePipe;
using VContainer;
using VContainer.Unity;
using System;
using Presentation.Gameplay.Views;
using R3;

namespace Presentation.Gameplay.Presenters
{
    public class GridPresenter : IInitializable, IDisposable
    {
        [Inject] private readonly IGridView _gridView;
        [Inject] private readonly IPublisher<BuildingPlaceRequestDTO> _placePublisher;
        [Inject] private readonly ISubscriber<BuildingPlacedDTO> _placedSubscriber;
        [Inject] private readonly ISubscriber<BuildingPlacementFailedDTO> _placementFailedSubscriber;
        [Inject] private readonly ISubscriber<BuildingSelectedDTO> _buildingSelectedSubscriber;

        private readonly CompositeDisposable _disposables = new();
        private BuildingType _selectedBuildingType = BuildingType.None;
        private int? _selectedBuildingId;

        public void Initialize()
        {
            // Input subscriptions
            _gridView.OnCellClicked
                .Subscribe(OnCellClicked)
                .AddTo(_disposables);

            // Business event subscriptions
            _placedSubscriber
                .Subscribe(OnBuildingPlaced)
                .AddTo(_disposables);

            _placementFailedSubscriber
                .Subscribe(OnPlacementFailed)
                .AddTo(_disposables);

            _buildingSelectedSubscriber
                .Subscribe(OnBuildingSelected)
                .AddTo(_disposables);
        }

        public void SelectBuildingType(BuildingType type)
        {
            _selectedBuildingType = type;
            _selectedBuildingId = null;
            _gridView.SetSelectionMode(SelectionMode.Placement);
        }

        private void OnCellClicked(GridPosition position)
        {
            if (_selectedBuildingType != BuildingType.None)
            {
                _placePublisher.Publish(new BuildingPlaceRequestDTO(_selectedBuildingType, position));
            }
        }

        private void OnBuildingPlaced(BuildingPlacedDTO dto)
        {
            _gridView.ShowBuilding(dto.Building);
            _gridView.ClearHighlights();
        }

        private void OnPlacementFailed(BuildingPlacementFailedDTO dto)
        {
            _gridView.ShowPlacementError(dto.Position, dto.Reason);
        }

        private void OnBuildingSelected(BuildingSelectedDTO dto)
        {
            _selectedBuildingId = dto.BuildingId;
            _selectedBuildingType = BuildingType.None;
            _gridView.SetSelectionMode(SelectionMode.BuildingSelected);
        }

        public void Dispose() => _disposables?.Dispose();
    }

    public enum SelectionMode { None, Placement, BuildingSelected }
}
