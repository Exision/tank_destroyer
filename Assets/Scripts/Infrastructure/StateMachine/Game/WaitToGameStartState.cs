using Infrastructure.StateMachine.Base;
using Services;
using UI;

namespace Infrastructure.StateMachine.Game
{
    public class WaitToGameStartState: IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IInputService _inputService;
        private readonly LoadingScreen _loadingScreen;

        public WaitToGameStartState(GameStateMachine gameStateMachine, IInputService inputService, LoadingScreen loadingScreen)
        {
            _gameStateMachine = gameStateMachine;
            _inputService = inputService;
            _loadingScreen = loadingScreen;
        }
        
        public void Exit()
        {
            _inputService.AnyKeyDown -= WaitToAnyKey;
        }

        public void Enter()
        {
            _loadingScreen.Show();
            _inputService.AnyKeyDown += WaitToAnyKey;
        }

        private void WaitToAnyKey()
        {
            _loadingScreen.Hide();
            _gameStateMachine.Enter<StartGameState>();
        }
    }
}