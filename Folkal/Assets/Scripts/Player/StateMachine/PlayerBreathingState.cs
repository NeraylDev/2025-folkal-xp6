using UnityEngine;

public class PlayerBreathingState : PlayerBaseState
{
    private float _breathingDuration;

    public PlayerBreathingState(PlayerStateMachine playerStateMachine, PlayerManager playerManager) : base(playerStateMachine, playerManager)
    {
        _breathingDuration = playerManager.GetPlayerBreathing.GetBreathingDuration;
    }

    public override void Enter()
    {
        GetPlayerManager.GetPlayerMovement.SetCanMove(false);
        GetPlayerManager.GetPlayerCamera.SetCameraEffects(60, 0, 0, _breathingDuration);
    }

    public override void Execute()
    {
        TryExit();
    }

    public override void TryExit()
    {
        if (GetPlayerManager.GetPlayerBreathing.IsBreathing == false)
        {
            GetPlayerStateMachine.SetState(GetPlayerStateMachine.GetState("Idle"));
        }
    }

    public override void Exit()
    {
        GetPlayerManager.GetPlayerMovement.SetCanMove(true);
    }
}
