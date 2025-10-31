using R3;
using Domain.Models;
using Domain.Enums;

namespace UseCases.Application
{
    public class BuildingStateObserver
    {
        private readonly ReactiveProperty<BuildingProcessStatus> _buildingStatus = new();
        public ReadOnlyReactiveProperty<BuildingProcessStatus> BuildingStatus => _buildingStatus;

        private BuildingModel _currentBuilding;

        public void ObserveBuilding(BuildingModel building)
        {
            _currentBuilding = building;

            // Подписываемся на изменения Domain модели
            building.ProcessStatusChanged += OnProcessStatusChanged;
            _buildingStatus.Value = building.ProcessStatus;
        }

        private void OnProcessStatusChanged(BuildingProcessStatus status)
        {
            _buildingStatus.Value = status;
        }

        public void StopObserving()
        {
            if (_currentBuilding != null)
            {
                _currentBuilding.ProcessStatusChanged -= OnProcessStatusChanged;
            }
        }
    }
}
