using System;
using System.Diagnostics;
using UnityEngine;

public class PlayerRunningState : PlayerBaseState
{
    public PlayerRunningState(PlayerStateMachine playerStateMachine, PlayerManager playerManager) : base(playerStateMachine, playerManager) { }

    public override void Enter()
    {
        GetPlayerManager.GetEvents.RaiseRunStart(GetPlayerManager);
        GetPlayerManager.GetPlayerCamera.SetCameraEffects
        (
            PlayerCamera.FOV.Running,
            PlayerCamera.Noise.Running
        );
    }

    public override void Execute()
    {
        if (GetPlayerManager.GetPlayerMovement.CanMove)
        {
            if (GetPlayerManager.GetPlayerMovement.GetInputDirection != Vector2.zero)
            {
                GetPlayerStateMachine.SetState(new PlayerWalkingState(GetPlayerStateMachine, GetPlayerManager));
            }
            else
            {
                GetPlayerStateMachine.SetState(new PlayerIdleState(GetPlayerStateMachine, GetPlayerManager));
            }
        }
        else
        {
            GetPlayerStateMachine.SetState(new PlayerIdleState(GetPlayerStateMachine, GetPlayerManager));
        }
    }

    public override void Exit()
    {
        GetPlayerManager.GetEvents.RaiseRunStop(GetPlayerManager);
    }

}