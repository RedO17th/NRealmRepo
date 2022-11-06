using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Functional.MashineState
{
    //For the mechanics of animation during the idle state,
    //one AnimationStatesController is enough.
    //But the state machine, in the future, could be used for other mechanics.

    public class HeaderItemStateMashine : BaseStateMachine
    {
        public HeaderItem Item { get; private set; } = null;

        protected override void InitializeStates()
        {
            _states = new List<BaseState>()
            {
                new BaseState(StateType.None, this),
                new IdleState(StateType.Idle, this)
            };
        }

        public override void Initialize(Component manager)
        {
            Item = manager as HeaderItem;
        }
    }    
}


