using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine, PlayerManager playerManager) : base(playerStateMachine, playerManager) { }

    public override void Enter()
    {
        GetPlayerManager.GetPlayerCamera.SetCameraEffects
        (
            PlayerCamera.FOV.Default,
            PlayerCamera.Noise.Default
        );
    }

    public override void Execute()
    {
        if (GetPlayerManager.GetPlayerMovement.CanMove == false)
            return;

        if (GetPlayerManager.GetPlayerThrowing.IsLoadingThrow)
        {
            GetPlayerStateMachine.SetState(new PlayerThrowingState(GetPlayerStateMachine, GetPlayerManager));
            return;
        }

        if (GetPlayerManager.GetPlayerMovement.GetInputDirection == Vector2.zero)
        {
            if (GetPlayerManager.GetPlayerMovement.IsRunning)
            {
                GetPlayerStateMachine.SetState(new PlayerRunningState(GetPlayerStateMachine, GetPlayerManager));
                return;
            }

            GetPlayerStateMachine.SetState(new PlayerWalkingState(GetPlayerStateMachine, GetPlayerManager));
        }
    }

}
