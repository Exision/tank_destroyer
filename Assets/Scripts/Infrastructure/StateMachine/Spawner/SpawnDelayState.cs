using System.Collections;
using Infrastructure.StateMachine.Base;
using Services;
using StaticData;
using UnityEngine;

namespace Infrastructure.StateMachine.Spawner
{
    public class SpawnDelayState: IState
    {
        private readonly SpawnerStateMachine _spawnerStateMachine;
        private readonly WaitForSeconds _waitForSpawn;
        private readonly ICoroutineRunner _coroutineRunner;

        private Coroutine _waitSpawnCoroutine;
        
        public SpawnDelayState(SpawnerStateMachine spawnerStateMachine, LevelStaticData levelData, ICoroutineRunner coroutineRunner)
        {
            _spawnerStateMachine = spawnerStateMachine;
            _coroutineRunner = coroutineRunner;

            _waitForSpawn = new WaitForSeconds(levelData.EnemySpawnDelay);
        }
        
        public void Exit()
        {
            if (_waitSpawnCoroutine != null)
                _coroutineRunner.StopCoroutine(_waitSpawnCoroutine);
        }

        public void Enter()
        {
            _waitSpawnCoroutine = _coroutineRunner.StartCoroutine(WaitToSpawn());
        }

        private IEnumerator WaitToSpawn()
        {
            yield return _waitForSpawn;

            _waitSpawnCoroutine = null;
            _spawnerStateMachine.Enter<SpawnEnemyState>();
        }
    }
}