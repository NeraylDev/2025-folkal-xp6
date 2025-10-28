using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerStateMachine
{
    private PlayerBaseState _currentState;

    public void Initialize(PlayerBaseState initialState)
    {
        _currentState = initialState;
        _currentState.Enter();
    }

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

    public void SetState(PlayerBaseState newState)
    {
        if (newState == null)
            return;

        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
