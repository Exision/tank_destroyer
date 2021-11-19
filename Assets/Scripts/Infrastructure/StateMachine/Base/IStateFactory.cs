namespace Infrastructure.StateMachine.Base
{
    public interface IStateFactory
    {
        T GetState<T>() where T : class, IExitable;
    }
}