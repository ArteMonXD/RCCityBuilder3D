using ContractsInterfaces;
using Domain.MessagesDTO;
using Domain.Models;
using Infrastructure;
using MessagePipe;
using VContainer;

namespace UseCases.Application
{
    public class RemoveBuildingUseCase : IMessageHandler<RemoveBuildingRequestDTO>
    {
        [Inject] private readonly IGameStateRepository _gameStateRepository;
        [Inject] private readonly IPublisher<BuildingRemovedDTO> _removedPublisher;
        [Inject] private readonly IPublisher<ResourceBalanceChangedDTO> _resourceChangedPublisher;
        [Inject] private readonly ILogger _logger;

        public void Handle(RemoveBuildingRequestDTO message)
        {
            _logger.Debug($"[RemoveBuildingUseCase] Handling removal for building {message.BuildingId}");

            var gameState = _gameStateRepository.GetCurrentState();
            var building = FindBuildingById(gameState, message.BuildingId);

            if (building == null)
            {
                return; // ѕросто игнорируем, если здание не найдено
            }

            // ”дал€ем здание и возвращаем часть стоимости
            var refund = gameState.BuildingCosts[building.Type] / 2;
            gameState.Buildings.Remove(building.Position);
            gameState.Gold += refund;
            _gameStateRepository.SaveState(gameState);

            _removedPublisher.Publish(new BuildingRemovedDTO(building.Id));
            _resourceChangedPublisher.Publish(new ResourceBalanceChangedDTO(gameState.Gold, refund));

            _logger.Information($"[RemoveBuildingUseCase] Building {building.Id} removed, refunded {refund} gold");
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