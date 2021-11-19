using Hero;
using Infrastructure.StateMachine.Base;
using Infrastructure.StateMachine.Spawner;

namespace Infrastructure.StateMachine.Game
{
    public class StartGameState: IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SpawnerStateMachine _spawnerStateMachine;
        private readonly PlayerHealth _player;

        public StartGameState(GameStateMachine gameStateMachine, SpawnerStateMachine spawnerStateMachine, PlayerHealth player)
        {
            _gameStateMachine = gameStateMachine;
            _spawnerStateMachine = spawnerStateMachine;
            _player = player;
        }

        public void Exit()
        {
            
        }

        public void Enter()
        {
            _player.gameObject.SetActive(true);
            _player.Reset();

            _spawnerStateMachine.Enter<SpawnerInitializeState>();
            _gameStateMachine.Enter<WaitToGameEndState>();
        }
    }
}