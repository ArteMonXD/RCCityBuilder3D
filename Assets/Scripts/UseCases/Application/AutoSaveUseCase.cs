using ContractsInterfaces;
using Cysharp.Threading.Tasks;
using Domain.MessagesDTO;
using MessagePipe;
using System.Threading;
using VContainer;
using VContainer.Unity;

namespace UseCases.Application
{
    public class AutoSaveUseCase : IInitializable, System.IDisposable
    {
        [Inject] private readonly IPublisher<SaveGameRequestDTO> _savePublisher;
        [Inject] private readonly ILogger _logger;

        private CancellationTokenSource _cancellationTokenSource;
        private bool _isRunning;

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            StartAutoSave().Forget();
        }

        private async UniTaskVoid StartAutoSave()
        {
            _isRunning = true;

            while (_isRunning && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    await UniTask.Delay(30000, cancellationToken: _cancellationTokenSource.Token); // 30 секунд

                    if (!_isRunning || _cancellationTokenSource.Token.IsCancellationRequested)
                        break;

                    _savePublisher.Publish(new SaveGameRequestDTO("autosave"));
                    _logger.Debug("[AutoSaveUseCase] Game auto-saved");
                }
                catch (System.OperationCanceledException)
                {
                    break;
                }
            }
        }

        public void Dispose()
        {
            _isRunning = false;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}
