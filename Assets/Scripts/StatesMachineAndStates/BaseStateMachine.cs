using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Functional.MashineState
{
    public enum StateType { None = -1, Idle }

    public abstract class BaseStateMachine : MonoBehaviour
    {
        [SerializeField] private StateType _startState = StateType.None;

        protected List<BaseState> _states = null;

        protected BaseState _currentState = null;
        protected StateType _newState = StateType.None;

        protected virtual void Awake() => InitializeStates();
        protected abstract void InitializeStates();

        public abstract void Initialize(Component manager);

        private void Start() => SetStartState();
        protected virtual void SetStartState()
        {
            _currentState = GetState(_startState);
        }

        protected BaseState GetState(StateType type)
        {
            return _states.FirstOrDefault(s => s.Type == type);
        }

        protected virtual void Update()
        {
            SetEmptyIfCurrentIsNull();

             _newState = (StateType)_currentState?.Tick();

            SwitchStates();
        }

        private void SetEmptyIfCurrentIsNull()
        {
            if (_currentState == null)
                _currentState = _states.First();
        }

        private void SwitchStates()
        {
            if (_newState != StateType.None && _newState != _currentState?.Type)
                _currentState = GetState(_newState);
        }
    }
}




