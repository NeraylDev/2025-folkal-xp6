using DG.Tweening;
using UnityEngine;

public class PlayerThrowingState : PlayerBaseState
{
    private float _throwChargeTimer;
    private float _throwChargeDuration;
    private float _throwChargeOffsetZ;

    private Tweener _throwChargingTween;

    public PlayerThrowingState(PlayerStateMachine playerStateMachine, PlayerManager playerManager) : base(playerStateMachine, playerManager)
    {
        _throwChargeDuration = playerManager.GetPlayerThrowing.GetThrowChargeDuration;
        _throwChargeOffsetZ = playerManager.GetPlayerThrowing.GetThrowChargeOffsetZ;
    }

    public override void Enter()
    {
        GetPlayerManager.GetEvents.RaiseThrowingStart(GetPlayerManager);

        GetPlayerManager.GetPlayerMovement.SetMoveSpeed(200f);
        GetPlayerManager.GetPlayerCamera.SetCameraEffects
        (
            PlayerCamera.FOV.Throwing,
            PlayerCamera.Noise.None
        );

        _throwChargeTimer = 0;
    }

    public override void Execute()
    {
        SetChargingTime();
        GetPlayerManager.GetPlayerThrowing.SetThrowingForce(_throwChargeTimer);

        float finalThrowableOffset = Mathf.Lerp(0, _throwChargeOffsetZ, _throwChargeTimer);
        GetPlayerManager.GetPlayerHand.SetOffsetZ(finalThrowableOffset);

        TryExit();
    }

    public override void TryExit()
    {
        if (GetPlayerManager.GetPlayerThrowing.HasThrewObject)
        {
            GetPlayerStateMachine.SetState(GetPlayerStateMachine.GetState("Idle"));
        }
    }

    public override void Exit()
    {
        if (_throwChargingTween != null)
        {
            _throwChargingTween.Kill();
            _throwChargingTween = null;
        }

        Throwable throwable = GetPlayerManager.GetPlayerHand.RemoveHeldThrowable();
        GetPlayerManager.GetPlayerThrowing.ApplyForce(throwable);

        GetPlayerManager.GetPlayerHand.SetOffsetZ(0);
        GetPlayerManager.GetEvents.RaiseThrow(GetPlayerManager);
    }


    private void SetChargingTime()
    {
        if (_throwChargingTween != null)
            return;

        Tweener throwChargingTween = DOTween.To
        (
            () => _throwChargeTimer,
            x => _throwChargeTimer = x,
            1, _throwChargeDuration
        )
        .SetEase(Ease.OutQuad);

        _throwChargingTween = throwChargingTween;
    }
}
