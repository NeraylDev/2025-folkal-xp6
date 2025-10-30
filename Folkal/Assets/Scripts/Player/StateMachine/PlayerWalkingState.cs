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
            GetPlayerStateMachine.SetState(new PlayerIdleState(GetPlayerStateMachine, GetPlayerManager));
            return;
        }

        if (GetPlayerManager.GetPlayerBreathing.IsBreathing)
        {
            GetPlayerStateMachine.SetState(new PlayerBreathingState(GetPlayerStateMachine, GetPlayerManager));
        }

        if (GetPlayerManager.GetPlayerThrowing.IsChargingThrow)
        {
            GetPlayerStateMachine.SetState(new PlayerThrowingState(GetPlayerStateMachine, GetPlayerManager));
            return;
        }

        if (GetPlayerManager.GetPlayerMovement.IsRunning)
        {
            GetPlayerStateMachine.SetState(new PlayerRunningState(GetPlayerStateMachine, GetPlayerManager));
        }
    }

}
