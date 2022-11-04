using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Functional.MashineState
{
    public enum StateType { None = -1, Idle }

    public class BaseStateMachine : MonoBehaviour
    {
        [SerializeField] private StateType _startState = StateType.None;

        private List<BaseState> _states = null;

        private BaseState _currentState = null;
        private StateType _newState = StateType.None;

        private void Awake()
        {
            _states = new List<BaseState>()
            {
                new BaseState(StateType.None, this),
                new IdleState(StateType.Idle, this)
            };
        }

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




