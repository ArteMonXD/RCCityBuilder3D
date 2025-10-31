using Domain.Enums;
using Domain.MessagesDTO;
using Infrastructure;
using MessagePipe;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Presentation.Gameplay.Presenters
{
    public class InputHandler : IInitializable, System.IDisposable
    {
        [Inject] private readonly IPublisher<BuildingPlaceRequestDTO> _buildingPlacePublisher;
        [Inject] private readonly IPublisher<BuildingSelectedDTO> _buildingSelectedPublisher;
        [Inject] private readonly IPublisher<RemoveBuildingRequestDTO> _removePublisher;
        [Inject] private readonly IGameStateRepository _gameStateRepository;

        private readonly CompositeDisposable _disposables = new();
        private BuildingType _selectedBuildingType = BuildingType.None;
        private int? _selectedBuildingId;

        public void Initialize()
        {
            Observable.EveryUpdate()
                .Subscribe(_ => HandleHotkeys())
                .AddTo(_disposables);
        }

        private void HandleHotkeys()
        {
            // Выбор зданий (1, 2, 3)
            if (Input.GetKeyDown(KeyCode.Alpha1)) SelectBuilding(BuildingType.House);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SelectBuilding(BuildingType.Farm);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SelectBuilding(BuildingType.Mine);

            // Отмена выбора
            if (Input.GetKeyDown(KeyCode.Escape)) DeselectAll();

            // Удаление выбранного здания
            if (Input.GetKeyDown(KeyCode.Delete) && _selectedBuildingId.HasValue)
            {
                _removePublisher.Publish(new RemoveBuildingRequestDTO(_selectedBuildingId.Value));
                DeselectAll();
            }
        }

        private void SelectBuilding(BuildingType buildingType)
        {
            _selectedBuildingType = buildingType;
            _selectedBuildingId = null;
            Debug.Log($"Selected building: {buildingType}");
        }

        private void DeselectAll()
        {
            _selectedBuildingType = BuildingType.None;
            _selectedBuildingId = null;
        }

        public void Dispose() => _disposables?.Dispose();
    }
}