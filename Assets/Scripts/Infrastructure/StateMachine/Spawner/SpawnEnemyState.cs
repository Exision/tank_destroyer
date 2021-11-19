using Enemy;
using Infrastructure.StateMachine.Base;
using Services;
using StaticData;

namespace Infrastructure.StateMachine.Spawner
{
    public class SpawnEnemyState: IState
    {
        private readonly SpawnerStateMachine _spawnerStateMachine;
        private readonly IEnemyFactory _enemyFactory;
        private readonly LevelStaticData _levelData;
        private readonly IRandomService _randomService;
        private readonly Logic.Spawner _spawner;
        
        public SpawnEnemyState(SpawnerStateMachine spawnerStateMachine, Logic.Spawner spawner, IEnemyFactory enemyFactory, LevelStaticData levelData, IRandomService randomService)
        {
            _spawnerStateMachine = spawnerStateMachine;
            _spawner = spawner;
            _enemyFactory = enemyFactory;
            _levelData = levelData;
            _randomService = randomService;
        }
        
        public void Exit()
        {
        }

        public void Enter()
        {
            if (_enemyFactory.GetActiveEnemyCount() >= _levelData.MaxEnemiesCount)
            {
                _spawnerStateMachine.Enter<WaitToSpawnState>();
            }
            else
            {
                SpawnEnemy();
                _spawnerStateMachine.Enter<SpawnDelayState>();
            }
        }

        private void SpawnEnemy()
        {
            EnemyStaticData enemyData = _levelData.LevelEnemies[_randomService.Next(0, _levelData.LevelEnemies.Length)];
            EnemyDeath enemy = _enemyFactory.Get(enemyData.EnemyTypeId);
            
            _spawner.Spawn(enemy);
        }
    }
}