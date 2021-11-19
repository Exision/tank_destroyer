using Infrastructure.StateMachine.Game;
using Services;
using UI;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class BootstrapInstaller: MonoInstaller<BootstrapInstaller>, IInitializable
    {
        [SerializeField] private LoadingScreen _loadingScreenPrefab;
        [SerializeField] private CoroutineRunner _coroutineRunnerPrefab;
        
        public override void InstallBindings()
        {
            BindInterfaces();
            
            BindInputService();
            BindRandomService();
            BuildGameMachineStateFactory();
        }

        public void Initialize()
        {
            BindCoroutineRunner();
            BindLoadingScreen();
            BindGameStateMachine();
            BindBaseGameStateMachineStates();
            LaunchGame();
        }

        private void BindInterfaces() => 
            Container.BindInterfacesTo<BootstrapInstaller>()
                .FromInstance(this)
                .AsSingle();

        private void BindInputService() =>
            Container.BindInterfacesAndSelfTo<IInputService>()
                .FromInstance(new KeyboardInputService())
                .AsSingle();

        private void BindRandomService() =>
            Container.Bind<IRandomService>()
                .FromInstance(new RandomService())
                .AsSingle();

        private void BuildGameMachineStateFactory() => 
            Container.Bind<GameStateFactory>()
                .FromInstance(new GameStateFactory())
                .AsSingle();

        private void BindCoroutineRunner()
        {
            CoroutineRunner coroutineRunner = Container
                .InstantiatePrefabForComponent<CoroutineRunner>(_coroutineRunnerPrefab);

            Container.Bind<ICoroutineRunner>()
                .FromInstance(coroutineRunner)
                .AsSingle();
        }

        private void BindLoadingScreen()
        {
            LoadingScreen loadingScreen = Container.InstantiatePrefabForComponent<LoadingScreen>(_loadingScreenPrefab);
            Container.Bind<LoadingScreen>().FromInstance(loadingScreen).AsSingle();
        }

        private void BindGameStateMachine()
        {
            GameStateFactory gameStateFactory = Container.Resolve<GameStateFactory>();
            gameStateFactory.AddBaseStates(Container);
            
            var gameStateMachine = new GameStateMachine(gameStateFactory);
            Container.Bind<GameStateMachine>().FromInstance(gameStateMachine).AsSingle();
        }

        private void BindBaseGameStateMachineStates()
        {
            var waitToGameStartState = new WaitToGameStartState(Container.Resolve<GameStateMachine>(), 
                Container.Resolve<IInputService>(), 
                Container.Resolve<LoadingScreen>());

            Container.Bind<WaitToGameStartState>().FromInstance(waitToGameStartState).AsSingle();
        }

        private void LaunchGame() => 
            Container.Resolve<GameStateMachine>()
                .Enter<WaitToGameStartState>();
    }
}