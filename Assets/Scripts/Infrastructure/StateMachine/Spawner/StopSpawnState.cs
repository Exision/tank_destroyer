using Infrastructure.StateMachine.Base;
using Services;

namespace Infrastructure.StateMachine.Spawner
{
    public class StopSpawnState: IState
    {
        private readonly IEnemyFactory _enemyFactory;

        public StopSpawnState(IEnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
        }
        
        public void Exit()
        {
        }

        public void Enter()
        {
            _enemyFactory.CleanUp();
        }
    }
}