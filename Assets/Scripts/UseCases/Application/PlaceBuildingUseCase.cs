using UnityEngine;
using MessagePipe;
using VContainer;
using Domain.MessagesDTO;
using Domain.Models;
using Domain.Enums;
using Infrastructure;
using ContractsInterfaces;

namespace UseCases.Application
{
    public class PlaceBuildingUseCase : IMessageHandler<BuildingPlaceRequestDTO>
    {
        [Inject] private readonly IPublisher<BuildingPlacedDTO> _placedPublisher;
        [Inject] private readonly IPublisher<BuildingPlacementFailedDTO> _placementFailedPublisher;
        [Inject] private readonly IPublisher<ResourceBalanceChangedDTO> _resourceChangedPublisher;
        [Inject] private readonly IGameStateRepository _gameStateRepository;
        [Inject] private readonly ContractsInterfaces.ILogger _logger;

        public void Handle(BuildingPlaceRequestDTO message)
        {
            _logger.Debug($"[PlaceBuildingUseCase] Handling placement for {message.BuildingType} at {message.Position}");

            var gameState = _gameStateRepository.GetCurrentState();

            // Validation
            if (!CanPlaceBuilding(gameState, message.Position, message.BuildingType))
            {
                _placementFailedPublisher.Publish(new BuildingPlacementFailedDTO(
                    "Cannot place building here", message.Position));
                return;
            }

            // Create building
            var building = new BuildingModel(GenerateId(), message.BuildingType, message.Position);
            gameState.Buildings[message.Position] = building;
            gameState.Gold -= gameState.BuildingCosts[message.BuildingType];

            _gameStateRepository.SaveState(gameState);

            // Notify success
            _placedPublisher.Publish(new BuildingPlacedDTO(building));
            _resourceChangedPublisher.Publish(new ResourceBalanceChangedDTO(
                gameState.Gold, -gameState.BuildingCosts[message.BuildingType]));

            _logger.Information($"[PlaceBuildingUseCase] Building placed: {building.Type} at {building.Position}");
        }

        private bool CanPlaceBuilding(GameStateModel gameState, GridPosition position, BuildingType type)
        {
            return !gameState.Buildings.ContainsKey(position) &&
                   gameState.Gold >= gameState.BuildingCosts[type] &&
                   position.X >= 0 && position.Z >= 0 && position.X < 32 && position.Z < 32;
        }

        private int GenerateId() => UnityEngine.Random.Range(1000, 9999);
    }
}
