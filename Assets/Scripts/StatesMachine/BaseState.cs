namespace Functional.MashineState
{
    public class BaseState
    {
        protected BaseStateMachine _stateMachine = null;

        public StateType Type { get; private set; } = StateType.None;

        public BaseState(StateType type, BaseStateMachine machine)
        {
            Type = type;
            _stateMachine = machine;
        }

        public virtual StateType Tick() { return StateType.None; }
    }
}




