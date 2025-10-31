using ContractsInterfaces;
using Domain.MessagesDTO;
using Domain.Models;
using Infrastructure;
using MessagePipe;
using VContainer;

namespace UseCases.Application
{
    public class UpgradeBuildingUseCase : IMessageHandler<UpgradeBuildingRequestDTO>
    {
        [Inject] private readonly IGameStateRepository _gameStateRepository;
        [Inject] private readonly IPublisher<BuildingUpgradedDTO> _upgradedPublisher;
        [Inject] private readonly IPublisher<UpgradeBuildingFailedDTO> _upgradeFailedPublisher;
        [Inject] private readonly IPublisher<ResourceBalanceChangedDTO> _resourceChangedPublisher;
        [Inject] private readonly ILogger _logger;

        public void Handle(UpgradeBuildingRequestDTO message)
        {
            _logger.Debug($"[UpgradeBuildingUseCase] Handling upgrade for building {message.BuildingId}");

            var gameState = _gameStateRepository.GetCurrentState();
            var building = FindBuildingById(gameState, message.BuildingId);

            if (building == null)
            {
                _upgradeFailedPublisher.Publish(new UpgradeBuildingFailedDTO(message.BuildingId, "Building not found"));
                return;
            }

            var upgradeCost = building.GetUpgradeCost();
            if (gameState.Gold < upgradeCost)
            {
                _upgradeFailedPublisher.Publish(new UpgradeBuildingFailedDTO(message.BuildingId, "Not enough gold"));
                return;
            }

            // Выполняем апгрейд
            building.Upgrade();
            gameState.Gold -= upgradeCost;
            _gameStateRepository.SaveState(gameState);

            _upgradedPublisher.Publish(new BuildingUpgradedDTO(building.Id, building.Level));
            _resourceChangedPublisher.Publish(new ResourceBalanceChangedDTO(gameState.Gold, -upgradeCost));

            _logger.Information($"[UpgradeBuildingUseCase] Building {building.Id} upgraded to level {building.Level}");
        }

        private BuildingModel FindBuildingById(GameStateModel gameState, int buildingId)
        {
            foreach (var building in gameState.Buildings.Values)
            {
                if (building.Id == buildingId)
                    return building;
            }
            return null;
        }
    }
}
