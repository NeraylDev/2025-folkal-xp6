using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public PlayerWalkingState(PlayerStateMachine playerStateMachine, PlayerManager playerManager) : base(playerStateMachine, playerManager) { }

    public override void Enter()
    {
        GetPlayerManager.GetEvents.RaiseWalkStart(GetPlayerManager);

        GetPlayerManager.GetPlayerMovement.ResetMoveSpeed();
        GetPlayerManager.GetPlayerCamera.SetCameraEffects
        (
            PlayerCamera.FOV.Default,
            PlayerCamera.Noise.Default
        );
    }

    public override void Execute()
    {
        if (GetPlayerManager.GetPlayerMovement.CanMove == false)
        {
            GetPlayerStateMachine.SetState(new PlayerIdleState(GetPlayerStateMachine, GetPlayerManager));
            return;
        }

        if (GetPlayerManager.GetPlayerThrowing.IsLoadingThrow)
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
