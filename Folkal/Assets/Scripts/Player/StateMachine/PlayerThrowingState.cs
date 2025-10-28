using UnityEngine;

public class PlayerThrowingState : PlayerBaseState
{
    public PlayerThrowingState(PlayerStateMachine playerStateMachine, PlayerManager playerManager) : base(playerStateMachine, playerManager) { }

    public override void Enter()
    {
        GetPlayerManager.GetEvents.RaiseThrowingStart(GetPlayerManager);

        GetPlayerManager.GetPlayerMovement.SetMoveSpeed(200f);
        GetPlayerManager.GetPlayerCamera.SetCameraEffects
        (
            PlayerCamera.FOV.Throwing,
            PlayerCamera.Noise.None
        );
    }

    public override void Execute()
    {
        if (GetPlayerManager.GetPlayerThrowing.HasThrewObject)
        {
            GetPlayerStateMachine.SetState(new PlayerIdleState(GetPlayerStateMachine, GetPlayerManager));
        }
    }

    public override void Exit()
    {
        GetPlayerManager.GetEvents.RaiseThrow(GetPlayerManager);
    }
}
