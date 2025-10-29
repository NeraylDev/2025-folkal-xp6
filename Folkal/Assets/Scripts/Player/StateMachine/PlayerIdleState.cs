using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine, PlayerManager playerManager) : base(playerStateMachine, playerManager) { }

    public override void Enter()
    {
        GetPlayerManager.GetPlayerCamera.SetCameraEffects(65, 0.4f, 0.35f);
    }

    public override void Execute()
    {
        TryExit();
    }

    public override void TryExit()
    {
        if (GetPlayerManager.GetPlayerMovement.CanMove == false)
            return;

        if (GetPlayerManager.GetPlayerBreathing.IsBreathing)
        {
            GetPlayerStateMachine.SetState(GetPlayerStateMachine.GetState("Breathing"));
            return;
        }

        if (GetPlayerManager.GetPlayerThrowing.IsChargingThrow)
        {
            GetPlayerStateMachine.SetState(GetPlayerStateMachine.GetState("Throwing"));
            return;
        }

        if (GetPlayerManager.GetPlayerMovement.GetInputDirection != Vector2.zero)
        {
            if (GetPlayerManager.GetPlayerMovement.IsRunning)
            {
                GetPlayerStateMachine.SetState(GetPlayerStateMachine.GetState("Running"));
                return;
            }

            GetPlayerStateMachine.SetState(GetPlayerStateMachine.GetState("Walking"));
        }
    }

}
