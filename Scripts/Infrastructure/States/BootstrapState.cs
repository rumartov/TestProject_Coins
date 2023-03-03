using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.LevelProgress;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.ResetLevel;
using Infrastructure.Services.SaveLoad;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly SceneLoader _sceneLoader;

        private readonly AllServices _services;

        private readonly GameStateMachine _stateMachine;

        private readonly ICoroutineRunner _coroutineRunner;
        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services,
            ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _coroutineRunner = coroutineRunner;
            
            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(Constants.InitialScene, EnterLoadLevel);
        }

        public void Exit()
        {

        }

        private void RegisterServices()
        {
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            
            _services.RegisterSingle<IResetLevelService>(
                new ResetLevelService(_services.Single<IPersistentProgressService>(), _stateMachine));

            _services.RegisterSingle<ILevelProgressService>(
                new LevelProgressService(_services.Single<IPersistentProgressService>(),
                    _services.Single<IResetLevelService>(),
                    _coroutineRunner,
                    _services.Single<IGameFactory>()));

            _services.RegisterSingle<IGameFactory>(
                new GameFactory(_services.Single<IAssetProvider>(), 
                    _services.Single<IPersistentProgressService>(),
                    _services.Single<IResetLevelService>(),
                    _services.Single<ILevelProgressService>()));
            
            _services.RegisterSingle<ISaveLoadService>(
                new SaveLoadService(_services.Single<IPersistentProgressService>(),
                    _services.Single<IGameFactory>()));
        }
        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }

    }
}
