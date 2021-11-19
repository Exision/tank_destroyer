namespace Infrastructure.StateMachine.Base
{
	public interface IPayloadedState<TPayload> : IExitable
	{
		void Enter(TPayload payload);
	}
}