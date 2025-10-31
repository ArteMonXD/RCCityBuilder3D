using Domain.Enums;
using Domain.MessagesDTO;
using MessagePipe;
using Presentation.Application.Views;
using R3;
using VContainer;
using VContainer.Unity;

namespace Presentation.Application.Presenters
{
    public class HudPresenter : IInitializable, System.IDisposable
    {
        [Inject] private readonly HudView _hudView;
        [Inject] private readonly IPublisher<BuildingPlaceRequestDTO> _buildingPlacePublisher;
        [Inject] private readonly IPublisher<UpgradeBuildingRequestDTO> _upgradePublisher;
        [Inject] private readonly IPublisher<MoveBuildingRequestDTO> _movePublisher;
        [Inject] private readonly IPublisher<RemoveBuildingRequestDTO> _removePublisher;
        [Inject] private readonly IPublisher<SaveGameRequestDTO> _savePublisher;
        [Inject] private readonly IPublisher<LoadGameRequestDTO> _loadPublisher;
        [Inject] private readonly ISubscriber<ResourceBalanceChangedDTO> _resourceChangedSubscriber;
        [Inject] private readonly ISubscriber<BuildingSelectedDTO> _buildingSelectedSubscriber;

        private readonly CompositeDisposable _disposables = new();
        private int? _selectedBuildingId;

        public void Initialize()
        {
            // Подписка на пользовательский ввод
            _hudView.OnBuildingSelected
                .Subscribe(OnBuildingSelected)
                .AddTo(_disposables);

            _hudView.OnBuildingAction
                .Subscribe(OnBuildingAction)
                .AddTo(_disposables);

            _hudView.OnSystemAction
                .Subscribe(OnSystemAction)
                .AddTo(_disposables);

            // Подписка на бизнес-события
            _resourceChangedSubscriber
                .Subscribe(OnResourceChanged)
                .AddTo(_disposables);

            _buildingSelectedSubscriber
                .Subscribe(OnBuildingSelected)
                .AddTo(_disposables);
        }

        private void OnBuildingSelected(BuildingType buildingType)
        {
            // Здесь будет логика выбора здания для строительства
            // Пока просто скрываем панель информации
            _hudView.HideBuildingInfo();
        }

        private void OnBuildingAction(BuildingAction action)
        {
            if (!_selectedBuildingId.HasValue) return;

            switch (action)
            {
                case BuildingAction.Upgrade:
                    _upgradePublisher.Publish(new UpgradeBuildingRequestDTO(_selectedBuildingId.Value));
                    break;
                case BuildingAction.Move:
                    // Активируем режим перемещения
                    break;
                case BuildingAction.Remove:
                    _removePublisher.Publish(new RemoveBuildingRequestDTO(_selectedBuildingId.Value));
                    break;
            }
        }

        private void OnSystemAction(SystemAction action)
        {
            switch (action)
            {
                case SystemAction.Save:
                    _savePublisher.Publish(new SaveGameRequestDTO());
                    break;
                case SystemAction.Load:
                    _loadPublisher.Publish(new LoadGameRequestDTO());
                    break;
            }
        }

        private void OnResourceChanged(ResourceBalanceChangedDTO dto)
        {
            _hudView.UpdateGold(dto.Gold);
        }

        private void OnBuildingSelected(BuildingSelectedDTO dto)
        {
            _selectedBuildingId = dto.BuildingId;
            // Здесь нужно получить информацию о здании и показать её
            _hudView.ShowBuildingInfo("Building", 1); // Заглушка
        }

        public void Dispose() => _disposables?.Dispose();
    }
}
