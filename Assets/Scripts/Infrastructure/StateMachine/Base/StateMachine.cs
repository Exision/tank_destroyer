namespace Infrastructure.StateMachine.Base
{
    public abstract class StateMachine
    {
        private readonly IStateFactory _stateFactory;
        private IExitable _activeState;
        
        protected StateMachine(IStateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }
        
        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }
        
        private TState ChangeState<TState>() where TState : class, IExitable
        {
            _activeState?.Exit();
      
            TState state = _stateFactory.GetState<TState>();
            _activeState = state;
      
            return state;
        }
    }
}