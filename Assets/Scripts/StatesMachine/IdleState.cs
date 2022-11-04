namespace Functional.MashineState
{
    public class IdleState : BaseState
    {
        public IdleState(StateType type, BaseStateMachine machine) : base(type, machine) 
        { }

        public override StateType Tick()
        {
            return StateType.Idle;
        }
    }
}




