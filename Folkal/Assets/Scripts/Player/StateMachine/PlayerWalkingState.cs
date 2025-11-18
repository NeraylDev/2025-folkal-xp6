using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public PlayerWalkingState(PlayerStateMachine playerStateMachine, PlayerManager playerManager) : base(playerStateMachine, playerManager) { }

    public override void Enter()
    {
        GetPlayerManager.GetEvents.RaiseWalkStart(GetPlayerManager);

        GetPlayerManager.GetPlayerMovement.ResetMoveSpeed();
        GetPlayerManager.GetPlayerCamera.SetCameraEffects(66, 0.25f, 1.25f);
    }

    public override void Execute()
    {
        TryExit();
    }

    public override void TryExit()
    {
        if (GetPlayerManager.GetPlayerMovement.CanMove == false
            || GetPlayerManager.GetPlayerMovement.GetInputDirection == Vector2.zero)
        {
            GetPlayerStateMachine.SetState(GetPlayerStateMachine.GetPlayerState("Idle"));
            return;
        }

        if (GetPlayerManager.GetPlayerBreathing.IsBreathing)
        {
            GetPlayerStateMachine.SetState(GetPlayerStateMachine.GetPlayerState("Breathing"));
        }

        if (GetPlayerManager.GetPlayerThrowing.IsChargingThrow)
        {
            GetPlayerStateMachine.SetState(GetPlayerStateMachine.GetPlayerState("Throwing"));
            return;
        }

        if (GetPlayerManager.GetPlayerMovement.IsRunning)
        {
            GetPlayerStateMachine.SetState(GetPlayerStateMachine.GetPlayerState("Running"));
        }
    }

    public override void Exit()
    {
        GetPlayerManager.GetEvents.RaiseWalkStop(GetPlayerManager);
    }

}
