using UnityEngine;

public class PlayerRunningState : PlayerBaseState
{
    public PlayerRunningState(PlayerStateMachine playerStateMachine, PlayerManager playerManager) : base(playerStateMachine, playerManager) { }

    public override void Enter()
    {
        GetPlayerManager.GetEvents.RaiseRunStart(GetPlayerManager); 
        
        GetPlayerManager.GetPlayerMovement.SetMoveSpeed(GetPlayerManager.GetPlayerMovement.GetRunningSpeed);
        GetPlayerManager.GetPlayerCamera.SetCameraEffects(72, 0.3f, 3.25f);
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

        if (GetPlayerManager.GetPlayerMovement.IsRunning == false)
        {
            GetPlayerStateMachine.SetState(GetPlayerStateMachine.GetPlayerState("Walking"));
        }
    }

    public override void Exit()
    {
        GetPlayerManager.GetEvents.RaiseRunStop(GetPlayerManager);
    }

}