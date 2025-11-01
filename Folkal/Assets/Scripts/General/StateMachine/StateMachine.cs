using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine
{
    protected IState _currentState;
    protected Dictionary<string, IState> _states = new Dictionary<string, IState>();

    public void Initialize(IState initialState)
    {
        InitializeStateDictionary(initialState);

        _currentState = initialState;
        _currentState.Enter();
    }

    protected abstract void InitializeStateDictionary(IState initialState);

    public void Execute()
    {
        if (_currentState != null)
        {
            _currentState.Execute();
        }
    }

    public void FixedExecute()
    {
        if (_currentState != null)
        {
            _currentState.FixedExecute();
        }
    }

    public void SetState(IState newState)
    {
        if (newState == null)
            return;

        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    protected IState GetState(string state)
    {
        if (_states.ContainsKey(state))
            return _states[state];

        return null;
    }
}
