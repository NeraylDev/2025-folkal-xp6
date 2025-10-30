using UnityEngine;

public class PlayerBreathingState : PlayerBaseState
{
    private float _totalBreathingDuration;
    private float _breathingInDuration;
    private float _breathingOutDuration;
    private bool _isBreathingOutActive;

    private float _breathingTimer;

    public PlayerBreathingState(PlayerStateMachine playerStateMachine, PlayerManager playerManager) : base(playerStateMachine, playerManager)
    {
        PlayerBreathing playerBreathing = playerManager.GetPlayerBreathing;

        _breathingInDuration = playerBreathing.GetBreathingInDuration;
        _breathingOutDuration = playerBreathing.GetBreathingOutDuration;
        _totalBreathingDuration = playerBreathing.GetBreathingDuration;
    }

    public override void Enter()
    {
        GetPlayerManager.GetEvents.RaiseBreathingStart(GetPlayerManager);

        GetPlayerManager.GetPlayerMovement.SetCanMove(false);
        GetPlayerManager.GetPlayerCamera.SetCameraEffects(60, 0, 0, _breathingInDuration);

        _isBreathingOutActive = false;
        _breathingTimer = 0;
    }

    public override void Execute()
    {
        if (_breathingTimer < _totalBreathingDuration)
        {
            if (_breathingTimer < _breathingInDuration)
            {
                _breathingTimer += Time.deltaTime;
            }
            else if (GetPlayerManager.GetPlayerBreathing.CanBreathOut == false)
            {
                GetPlayerManager.GetPlayerBreathing.SetCanBreathOut(true);
            }
            else if (GetPlayerManager.GetPlayerBreathing.IsBreathingOut)
            {
                if (_isBreathingOutActive == false)
                {
                    GetPlayerManager.GetPlayerCamera.SetCameraEffects(65, 0.4f, 0.35f, _breathingOutDuration);
                    GetPlayerManager.GetEvents.RaiseBreathingStop(GetPlayerManager);

                    _isBreathingOutActive = true;
                }

                _breathingTimer += Time.deltaTime;
            }
        }
        else
        {
            GetPlayerManager.GetPlayerBreathing.SetIsBreathing(false);
        }

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
        GetPlayerManager.GetPlayerBreathing.StopBreath();
        GetPlayerManager.GetPlayerMovement.SetCanMove(true);
    }
}
