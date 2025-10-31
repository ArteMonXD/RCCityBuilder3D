using ContractsInterfaces;
using Cysharp.Threading.Tasks;
using Domain.MessagesDTO;
using Domain.Models;
using Infrastructure;
using MessagePipe;
using R3;
using System;
using System.Threading;
using VContainer;
using VContainer.Unity;

namespace UseCases.Application
{
    public class EconomyUseCase : IInitializable, IDisposable
    {
        [Inject] private readonly IGameStateRepository _gameStateRepository;
        [Inject] private readonly IPublisher<ResourceBalanceChangedDTO> _resourceChangedPublisher;
        [Inject] private readonly IPublisher<PassiveIncomeDTO> _passiveIncomePublisher;
        [Inject] private readonly ILogger _logger;

        private readonly CompositeDisposable _disposables = new();
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isRunning;

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            StartPassiveIncome().Forget();
        }

        private async UniTaskVoid StartPassiveIncome()
        {
            _isRunning = true;

            while (_isRunning && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: _cancellationTokenSource.Token);

                    if (!_isRunning || _cancellationTokenSource.Token.IsCancellationRequested)
                        break;

                    var gameState = _gameStateRepository.GetCurrentState();
                    var totalIncome = CalculateTotalIncome(gameState);

                    if (totalIncome > 0)
                    {
                        gameState.Gold += totalIncome;
                        _gameStateRepository.SaveState(gameState);

                        _resourceChangedPublisher.Publish(new ResourceBalanceChangedDTO(gameState.Gold, totalIncome));
                        _passiveIncomePublisher.Publish(new PassiveIncomeDTO(totalIncome));

                        _logger.Debug($"[EconomyUseCase] Passive income: +{totalIncome} Gold");
                    }
                }
                catch (OperationCanceledException)
                {
                    // Это нормально при отмене
                    break;
                }
            }
        }

        private int CalculateTotalIncome(GameStateModel gameState)
        {
            int total = 0;
            foreach (var building in gameState.Buildings.Values)
            {
                total += building.GetIncome();
            }
            return total;
        }

        public void Dispose()
        {
            _isRunning = false;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _disposables?.Dispose();
        }
    }
}
