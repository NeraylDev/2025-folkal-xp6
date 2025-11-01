using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerStateMachine : StateMachine
{
    protected override void InitializeStateDictionary(IState initialState)
    {
        PlayerBaseState state = initialState as PlayerBaseState;
        PlayerManager playerManager = state.GetPlayerManager;
        _states.Add("Idle", new PlayerIdleState(this, playerManager));
        _states.Add("Walking", new PlayerWalkingState(this, playerManager));
        _states.Add("Running", new PlayerRunningState(this, playerManager));
        _states.Add("Throwing", new PlayerThrowingState(this, playerManager));
        _states.Add("Breathing", new PlayerBreathingState(this, playerManager));
    }

    public PlayerBaseState GetPlayerState(string state)
    {
        IState temp = base.GetState(state);
        if (temp == null)
            return null;

        return temp as PlayerBaseState;
    }
}
