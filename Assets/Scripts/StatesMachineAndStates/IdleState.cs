using UnityEngine;

namespace Functional.MashineState
{
    public class IdleState : BaseState
    {
        private HeaderItemStateMashine _mashine = null;

        public IdleState(StateType type, BaseStateMachine machine) : base(type, machine) 
        {
            _mashine = machine as HeaderItemStateMashine;
        }

        public override StateType Tick() => StateType.Idle;
    }
}




