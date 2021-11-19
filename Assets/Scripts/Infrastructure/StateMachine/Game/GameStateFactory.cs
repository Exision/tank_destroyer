using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Base;
using Zenject;

namespace Infrastructure.StateMachine.Game
{
    public class GameStateFactory: IStateFactory
    {
        private readonly Dictionary<Type, Func<IExitable>> _states;

        public GameStateFactory()
        {
            _states = new Dictionary<Type, Func<IExitable>>();
        }

        public void AddBaseStates(DiContainer container)
        {
            _states.Add(typeof(WaitToGameStartState), container.Resolve<WaitToGameStartState>);
        }

        public void AddAdditionalStates(DiContainer container)
        {
            _states.Add(typeof(StartGameState), container.Resolve<StartGameState>);
            _states.Add(typeof(WaitToGameEndState), container.Resolve<WaitToGameEndState>);
        }
        
        public TState GetState<TState>() where TState : class, IExitable => 
            Create(typeof(TState)) as TState;

        private IExitable Create(Type type)
        {
            if (!_states.TryGetValue(type, out Func<IExitable> state))
                throw new Exception($"State {type.Name} not found");

            return state();
        }
    }
}