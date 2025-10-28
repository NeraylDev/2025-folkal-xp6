using System;
using System.Diagnostics;
using UnityEngine;

public class PlayerRunningState : PlayerBaseState
{
    public PlayerRunningState(PlayerStateMachine playerStateMachine, PlayerManager playerManager) : base(playerStateMachine, playerManager) { }

    public override void Enter()
    {
        GetPlayerManager.GetEvents.RaiseRunStart(GetPlayerManager);

        GetPlayerManager.GetPlayerMovement.SetMoveSpeed(375f);
        GetPlayerManager.GetPlayerCamera.SetCameraEffects
        (
            PlayerCamera.FOV.Running,
            PlayerCamera.Noise.Running
        );
    }

    public override void Execute()
    {
        if (GetPlayerManager.GetPlayerMovement.CanMove == false
            || GetPlayerManager.GetPlayerMovement.GetInputDirection == Vector2.zero)
        {
            GetPlayerStateMachine.SetState(new PlayerIdleState(GetPlayerStateMachine, GetPlayerManager));
            return;
        }

        if (GetPlayerManager.GetPlayerThrowing.IsLoadingThrow)
        {
            GetPlayerStateMachine.SetState(new PlayerThrowingState(GetPlayerStateMachine, GetPlayerManager));
            return;
        }

        if (GetPlayerManager.GetPlayerMovement.IsRunning == false)
        {
            GetPlayerStateMachine.SetState(new PlayerWalkingState(GetPlayerStateMachine, GetPlayerManager));
        }
    }

    public override void Exit()
    {
        GetPlayerManager.GetEvents.RaiseRunStop(GetPlayerManager);
    }

}