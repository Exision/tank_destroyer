using Infrastructure.StateMachine.Base;

namespace Infrastructure.StateMachine.Spawner
{
    public class SpawnerStateMachine: Base.StateMachine
    {
        public SpawnerStateMachine(IStateFactory stateFactory) : base(stateFactory)
        {
        }
    }
}