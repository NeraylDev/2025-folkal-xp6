using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerStateMachine
{
    private PlayerBaseState _currentState;
    private Dictionary<string, PlayerBaseState> _states = new Dictionary<string, PlayerBaseState>();

    public void Initialize(PlayerBaseState initialState)
    {
        PlayerManager playerManager = initialState.GetPlayerManager;
        _states.Add("Idle", new PlayerIdleState(this, playerManager));
        _states.Add("Walking", new PlayerWalkingState(this, playerManager));
        _states.Add("Running", new PlayerRunningState(this, playerManager));
        _states.Add("Throwing", new PlayerThrowingState(this, playerManager));
        _states.Add("Breathing", new PlayerBreathingState(this, playerManager));

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

    public PlayerBaseState GetState(string state)
    {
        if (_states.ContainsKey(state))
            return _states[state];

        return null;
    }
}
