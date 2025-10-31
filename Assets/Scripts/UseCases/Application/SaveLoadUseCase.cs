using Domain.MessagesDTO;
using Infrastructure;
using MessagePipe;
using System;
using UnityEngine;
using VContainer;
using ContractsInterfaces;

namespace UseCases.Application
{
    public class SaveLoadUseCase : IMessageHandler<SaveGameRequestDTO>, IMessageHandler<LoadGameRequestDTO>
    {
        [Inject] private readonly IGameStateRepository _gameStateRepository;
        [Inject] private readonly IPublisher<GameSavedDTO> _gameSavedPublisher;
        [Inject] private readonly IPublisher<GameLoadedDTO> _gameLoadedPublisher;
        [Inject] private readonly ContractsInterfaces.ILogger _logger;

        public void Handle(SaveGameRequestDTO message)
        {
            _gameStateRepository.SaveToDisk(message.SaveSlot);
            _gameSavedPublisher.Publish(new GameSavedDTO(message.SaveSlot, DateTime.UtcNow));
            _logger.Debug($"[SaveLoadUseCase] Game saved: {message.SaveSlot}");
        }

        public void Handle(LoadGameRequestDTO message)
        {
            if (_gameStateRepository.LoadFromDisk(message.SaveSlot))
            {
                _gameLoadedPublisher.Publish(new GameLoadedDTO(_gameStateRepository.GetCurrentState()));
                _logger.Debug($"[SaveLoadUseCase] Game loaded: {message.SaveSlot}");
            }
            else
            {
                _logger.Warning($"[SaveLoadUseCase] Failed to load: {message.SaveSlot}");
            }
        }
    }
}
