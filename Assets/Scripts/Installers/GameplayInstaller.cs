using ContractsInterfaces;
using Infrastructure;
using MessagePipe;
using Presentation.Application.Presenters;
using Presentation.Application.Views;
using Presentation.Gameplay.Presenters;
using Presentation.Gameplay.Views;
using System;
using UseCases.Application;
using VContainer;
using VContainer.Unity;

namespace Installers
{
    public class GameplayInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Регистрируем сервисы вручную
            RegisterServices(builder);
            RegisterMessageHandlers(builder);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            // Logger
            builder.Register<UnityLogger>(Lifetime.Singleton).As<ILogger>();

            // Infrastructure
            builder.Register<GameStateRepository>(Lifetime.Singleton).As<IGameStateRepository>();

            // Use Cases
            builder.Register<PlaceBuildingUseCase>(Lifetime.Singleton);
            builder.Register<EconomyUseCase>(Lifetime.Singleton);
            builder.Register<SaveLoadUseCase>(Lifetime.Singleton);
            builder.Register<UpgradeBuildingUseCase>(Lifetime.Singleton);
            builder.Register<MoveBuildingUseCase>(Lifetime.Singleton);
            builder.Register<RemoveBuildingUseCase>(Lifetime.Singleton);
            builder.Register<AutoSaveUseCase>(Lifetime.Singleton);

            // Presenters
            builder.Register<GridPresenter>(Lifetime.Singleton);
            builder.Register<HudPresenter>(Lifetime.Singleton);
            builder.Register<InputHandler>(Lifetime.Singleton);

            // Views
            builder.RegisterComponentInHierarchy<GridView>().As<IGridView>();
            builder.RegisterComponentInHierarchy<HudView>().AsSelf();
            builder.RegisterComponentInHierarchy<BuildingGhostView>().AsSelf();
            builder.RegisterComponentInHierarchy<CameraController>().AsSelf();
        }

        private void RegisterMessageHandlers(IContainerBuilder builder)
        {
            // Регистрируем все UseCases как обработчики сообщений
            builder.RegisterBuildCallback(container =>
            {
                // Создаем адаптер для преобразования IObjectResolver в IServiceProvider
                var serviceProvider = new VContainerServiceProvider(container);
                SetupGlobalMessagePipe(serviceProvider);
            });
        }

        private void SetupGlobalMessagePipe(IServiceProvider serviceProvider)
        {
            // Устанавливаем провайдер для GlobalMessagePipe
            GlobalMessagePipe.SetProvider(serviceProvider);
        }
    }

    // Адаптер для преобразования VContainer.IObjectResolver в System.IServiceProvider
    public class VContainerServiceProvider : IServiceProvider
    {
        private readonly IObjectResolver _container;

        public VContainerServiceProvider(IObjectResolver container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (Exception)
            {
                // Если сервис не зарегистрирован, возвращаем null (стандартное поведение IServiceProvider)
                return null;
            }
        }
    }
}
