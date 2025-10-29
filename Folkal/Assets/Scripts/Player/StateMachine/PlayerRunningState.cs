using UnityEngine;

public class PlayerRunningState : PlayerBaseState
{
    public PlayerRunningState(PlayerStateMachine playerStateMachine, PlayerManager playerManager) : base(playerStateMachine, playerManager) { }

    public override void Enter()
    {
        GetPlayerManager.GetEvents.RaiseRunStart(GetPlayerManager);

        GetPlayerManager.GetPlayerMovement.SetMoveSpeed(375f);
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
            GetPlayerStateMachine.SetState(new PlayerIdleState(GetPlayerStateMachine, GetPlayerManager));
            return;
        }

        if (GetPlayerManager.GetPlayerThrowing.IsChargingThrow)
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