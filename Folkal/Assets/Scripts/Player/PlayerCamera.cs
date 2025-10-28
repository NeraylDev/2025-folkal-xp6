using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : PlayerSubsystem
{
    public enum FOV
    {
        Default = 65,
        Running = 72,
        Throwing = 61
    }

    public enum Noise
    {
        None,
        Default,
        Running
    }

    [Header("Interaction Settings")]
    [SerializeField] private float _interactionDistance;
    [SerializeField] private LayerMask _interactableMask;

    [Header("Camera Noise")]
    [SerializeField] private NoiseSettings[] _noiseProfiles;
    private Noise _currentNoise;
    private List<Tweener> _activeTweeners = new List<Tweener>();
    private float _timeToStopShaking;
    private float _currentTimeToStopShaking;
    private bool _isShaking;

    private CinemachineCamera _cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin _cinemachineNoise;
    
    private void Awake()
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();
        _cinemachineNoise = _cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        UpdateCameraRaycast();

        if (_isShaking)
        {
            _currentTimeToStopShaking += Time.deltaTime;
            if (_currentTimeToStopShaking >= _timeToStopShaking)
            {
                _isShaking = false;
                DeactivateCameraShake();
            }
        }
    }

    protected override void SetEvents(InputActionAsset actionAsset)
    {
        _playerManager.GetEvents.onThrowingStart += (PlayerManager playerManager)
            => SetCameraEffects(FOV.Throwing, Noise.None);
        _playerManager.GetEvents.onThrow += (PlayerManager playerManager)
            => SetCameraEffects(FOV.Default, Noise.Default);

        /*_playerManager.GetEvents.onRunStart += (PlayerManager playerManager)
            => SetCameraEffects(FOV.Running, Noise.Running);
        _playerManager.GetEvents.onRunStop += (PlayerManager playerManager)
            => SetCameraEffects(FOV.Default, Noise.Default);*/
    }

    private void UpdateCameraRaycast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _interactableMask)
            && !_playerManager.GetPlayerHand.IsHoldingThrowable)
        {
            HUDManager.instance.SetCrosshair(HUDManager.CrosshairType.Interaction);
        }
        else
        {
            HUDManager.instance.SetCrosshair(HUDManager.CrosshairType.Default);
        }
    }

    public void SetCameraShake(float amplitude, float frequency, float duration)
    {
        if (_noiseProfiles != null)
            _cinemachineNoise.NoiseProfile = _noiseProfiles[1];

        _cinemachineNoise.AmplitudeGain = amplitude;
        _cinemachineNoise.FrequencyGain = frequency;
        _timeToStopShaking = duration;
        _currentTimeToStopShaking = 0;

        _isShaking = true;
    }

    public void DeactivateCameraShake()
    {
        if (_noiseProfiles != null)
            _cinemachineNoise.NoiseProfile = _noiseProfiles[0];

        SetCameraNoise(_currentNoise);
    }

    public void SetCameraEffects(FOV fov, Noise noise)
    {
        _activeTweeners.ForEach((x) => x.Kill());
        _activeTweeners.Clear();

        SetCameraFOV(fov);
        SetCameraNoise(noise);
    }

    private void SetCameraFOV(FOV fov)
    {
        float transitionDuration = 0.25f;
        switch (fov)
        {
            case FOV.Throwing:
                transitionDuration = 1f;
                break;
        }

        Tweener fovTween = DOTween.To
        (
            () => _cinemachineCamera.Lens.FieldOfView,
            x => _cinemachineCamera.Lens.FieldOfView = x,
            (int)fov,
            transitionDuration
        );

        _activeTweeners.Add(fovTween);
    }

    private void SetCameraNoise(Noise noise)
    {
        if (_currentNoise != noise)
            _currentNoise = noise;

        if (_isShaking || _cinemachineNoise == null)
            return;

        float amplitudeGain = 0;
        float frequencyGain = 0;
        float transitionDuration = 0.25f;
        switch (_currentNoise)
        {
            case Noise.Default:
                amplitudeGain = 0.4f;
                frequencyGain = 0.35f;
                break;

            case Noise.Running:
                amplitudeGain = 0.35f;
                frequencyGain = 3.5f;
                break;
        }

        Tweener amplitudeTween = DOTween.To
        (
            () => _cinemachineNoise.AmplitudeGain,
            x => _cinemachineNoise.AmplitudeGain = x,
            amplitudeGain,
            transitionDuration
        );

        Tweener frequencyTweener = DOTween.To
        (
            () => _cinemachineNoise.FrequencyGain,
            x => _cinemachineNoise.FrequencyGain = x,
            frequencyGain,
            transitionDuration
        );

        _activeTweeners.Add(amplitudeTween);
        _activeTweeners.Add(frequencyTweener);
    }

}
