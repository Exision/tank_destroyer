using Infrastructure.StateMachine.Base;
using Services;

namespace Infrastructure.StateMachine.Spawner
{
    public class WaitToSpawnState: IState
    {
        private readonly SpawnerStateMachine _spawnerStateMachine;
        private readonly IEnemyFactory _enemyFactory;

        public WaitToSpawnState(SpawnerStateMachine spawnerStateMachine, IEnemyFactory enemyFactory)
        {
            _spawnerStateMachine = spawnerStateMachine;
            _enemyFactory = enemyFactory;
        }
        
        public void Exit()
        {
        }

        public void Enter()
        {
            _enemyFactory.EnemyDied += OnEnemyDied;
        }

        private void OnEnemyDied()
        {
            _enemyFactory.EnemyDied -= OnEnemyDied;
            _spawnerStateMachine.Enter<SpawnEnemyState>();
        }
    }
}