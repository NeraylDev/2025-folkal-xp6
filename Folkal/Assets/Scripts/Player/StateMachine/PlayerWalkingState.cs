using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public PlayerWalkingState(PlayerStateMachine playerStateMachine, PlayerManager playerManager) : base(playerStateMachine, playerManager) { }

    public override void Enter()
    {
        GetPlayerManager.GetEvents.RaiseWalkStart(GetPlayerManager);
        GetPlayerManager.GetPlayerCamera.SetCameraEffects
        (
            PlayerCamera.FOV.Default,
            PlayerCamera.Noise.Default
        );
    }

    public override void Execute()
    {
        if (GetPlayerManager.GetPlayerMovement.IsRunning)
        {
            GetPlayerStateMachine.SetState(new PlayerRunningState(GetPlayerStateMachine, GetPlayerManager));
        }
        else if (GetPlayerManager.GetPlayerMovement.GetInputDirection == Vector2.zero)
        {
            GetPlayerStateMachine.SetState(new PlayerIdleState(GetPlayerStateMachine, GetPlayerManager));
        }
    }

}
