using Hero;
using Infrastructure.StateMachine.Base;
using Infrastructure.StateMachine.Spawner;
using StaticData;
using UnityEngine;

namespace Infrastructure.StateMachine.Game
{
    public class WaitToGameEndState: IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SpawnerStateMachine _spawnerStateMachine;
        private readonly PlayerDeath _player;
        private readonly LevelStaticData _levelData;
        
        public WaitToGameEndState(GameStateMachine gameStateMachine, SpawnerStateMachine spawnerStateMachine, PlayerHealth player, LevelStaticData levelData)
        {
            _gameStateMachine = gameStateMachine;
            _spawnerStateMachine = spawnerStateMachine;
            _player = player.GetComponent<PlayerDeath>();
            _levelData = levelData;
        }

        public void Exit()
        {
            _player.Died -= OnPlayerDied;
        }

        public void Enter()
        {
            _player.Died += OnPlayerDied;
        }

        private void OnPlayerDied()
        {
            _player.gameObject.SetActive(false);
            _player.transform.SetPositionAndRotation(_levelData.PlayerStartPoint, Quaternion.identity);
            
            _spawnerStateMachine.Enter<StopSpawnState>();
            _gameStateMachine.Enter<WaitToGameStartState>();
        }
    }
}