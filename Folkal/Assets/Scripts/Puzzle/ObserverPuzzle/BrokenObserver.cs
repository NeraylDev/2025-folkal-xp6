using System;
using DG.Tweening;
using UnityEngine;

public class BrokenObserver : Observer
{
    [SerializeField] private string _mentalPresenceID;
    [SerializeField] private string _observedID;
    
    private Renderer _renderer;
    private MaterialPropertyBlock _materialPropertyBlock;

    private float _mentalPresenceFactor;
    private float _observedFactor;
    private Tweener _mentalPresenceTween;
    private Tweener _observedTween;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnActivated(PlayerManager playerManager)
    {
        if (_mentalPresenceTween != null)
        {
            Tweener temp = _mentalPresenceTween;
            _mentalPresenceTween = null;
            temp.Kill();
        }

        _mentalPresenceTween = DOTween.To
        (
            () => _mentalPresenceFactor,
            x => _mentalPresenceFactor = x,
            1f,
            playerManager.GetPlayerBreathing.GetBreathingOutDuration
        )
        .OnUpdate(UpdateMaterial);
    }

    protected override void OnDeactivated(PlayerManager playerManager)
    {
        if (_mentalPresenceTween != null)
        {
            Tweener temp = _mentalPresenceTween;
            _mentalPresenceTween = null;
            temp.Kill();
        }

        _mentalPresenceTween = DOTween.To
        (
            () => _mentalPresenceFactor,
            x => _mentalPresenceFactor = x,
            0f,
            1f
        )
        .OnUpdate(UpdateMaterial);
    }

    protected override void OnObserved()
    {
        if (_observedTween != null)
        {
            Tweener temp = _observedTween;
            _observedTween = null;
            temp.Kill();
        }

        _observedTween = DOTween.To
        (
            () => _observedFactor,
            x => _observedFactor = x,
            1f,
            0.5f
        )
        .OnUpdate(UpdateMaterial);
    }

    private void UpdateMaterial()
    {
        _materialPropertyBlock = new MaterialPropertyBlock();
        _materialPropertyBlock.SetFloat(_mentalPresenceID, _mentalPresenceFactor);
        _materialPropertyBlock.SetFloat(_observedID, _observedFactor);

        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
