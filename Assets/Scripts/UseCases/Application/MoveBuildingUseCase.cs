using ContractsInterfaces;
using Domain.MessagesDTO;
using Domain.Models;
using Infrastructure;
using MessagePipe;
using VContainer;

namespace UseCases.Application
{
    public class MoveBuildingUseCase : IMessageHandler<MoveBuildingRequestDTO>
    {
        [Inject] private readonly IGameStateRepository _gameStateRepository;
        [Inject] private readonly IPublisher<BuildingMovedDTO> _movedPublisher;
        [Inject] private readonly IPublisher<MoveBuildingFailedDTO> _moveFailedPublisher;
        [Inject] private readonly ILogger _logger;

        public void Handle(MoveBuildingRequestDTO message)
        {
            _logger.Debug($"[MoveBuildingUseCase] Handling move for building {message.BuildingId} to {message.NewPosition}");

            var gameState = _gameStateRepository.GetCurrentState();
            var building = FindBuildingById(gameState, message.BuildingId);

            if (building == null)
            {
                _moveFailedPublisher.Publish(new MoveBuildingFailedDTO(message.BuildingId, "Building not found"));
                return;
            }

            if (gameState.Buildings.ContainsKey(message.NewPosition))
            {
                _moveFailedPublisher.Publish(new MoveBuildingFailedDTO(message.BuildingId, "Position occupied"));
                return;
            }

            // Выполняем перемещение
            var oldPosition = building.Position;
            gameState.Buildings.Remove(oldPosition);
            building.MoveTo(message.NewPosition);
            gameState.Buildings[message.NewPosition] = building;
            _gameStateRepository.SaveState(gameState);

            _movedPublisher.Publish(new BuildingMovedDTO(building.Id, oldPosition, message.NewPosition));
            _logger.Information($"[MoveBuildingUseCase] Building {building.Id} moved from {oldPosition} to {message.NewPosition}");
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
