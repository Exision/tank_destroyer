using Infrastructure.StateMachine.Base;
using Services;
using StaticData;

namespace Infrastructure.StateMachine.Spawner
{
    public class SpawnerInitializeState : IState
    {
        private readonly SpawnerStateMachine _stateMachine;
        private readonly IEnemyFactory _enemyFactory;
        private readonly LevelStaticData _levelStaticData;

        public SpawnerInitializeState(SpawnerStateMachine stateMachine, LevelStaticData levelStaticData, IEnemyFactory enemyFactory)
        {
            _stateMachine = stateMachine;
            _enemyFactory = enemyFactory;
            _levelStaticData = levelStaticData;
        }

        public void Exit()
        {
        }

        public void Enter()
        {
            _enemyFactory.Initialize(_levelStaticData.LevelEnemies, _levelStaticData.MaxEnemiesCount);
            _stateMachine.Enter<SpawnEnemyState>();
        }
    }
}