namespace Infrastructure.StateMachine.Base
{
	public interface IState : IExitable
	{
		void Enter();
	}
}