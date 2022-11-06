namespace Functional.MashineState
{
    public class BaseState
    {
        public StateType Type { get; private set; } = StateType.None;

        public BaseState(StateType type, BaseStateMachine machine)
        {
            Type = type;
        }

        public virtual StateType Tick() { return StateType.None; }
    }
}




