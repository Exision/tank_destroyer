using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Base;
using Zenject;

namespace Infrastructure.StateMachine.Spawner
{
    public class SpawnerStateFactory: IStateFactory
    {
        private readonly Dictionary<Type, Func<IExitable>> _states;

        public SpawnerStateFactory(DiContainer container)
        {
            _states = new Dictionary<Type, Func<IExitable>>()
            {
                [typeof(SpawnerInitializeState)] = container.Resolve<SpawnerInitializeState>, 
                [typeof(SpawnEnemyState)] = container.Resolve<SpawnEnemyState>,
                [typeof(SpawnDelayState)] = container.Resolve<SpawnDelayState>,
                [typeof(WaitToSpawnState)] = container.Resolve<WaitToSpawnState>,
                [typeof(StopSpawnState)] = container.Resolve<StopSpawnState>
            };
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