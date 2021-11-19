using Infrastructure.StateMachine.Base;

namespace Infrastructure.StateMachine.Game
{
    public class GameStateMachine: Base.StateMachine
    {
        public GameStateMachine(IStateFactory stateFactory) : base(stateFactory)
        {
        }
    }
}