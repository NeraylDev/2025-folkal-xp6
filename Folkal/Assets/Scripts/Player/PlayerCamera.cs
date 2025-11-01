using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : PlayerSubsystem
{
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin _cinemachineNoise;
    private Camera _camera;

    [Header("Interaction Settings")]
    [SerializeField] private float _interactionDistance;
    [SerializeField] private LayerMask _interactableMask;

    [Header("Noise Settings")]
    [SerializeField] private NoiseSettings[] _noiseProfiles;
    private float _currentNoiseAmplitude;
    private float _currentNoiseFrequency;
    
    private float _timeToStopShaking;
    private float _currentTimeToStopShaking;
    private bool _isShaking;

    private Color _defaultBackgroundColor;
    
    private List<Tweener> _activeTweeners = new List<Tweener>();

    public Transform GetCameraTransform => _cinemachineCamera.transform;


    #region MonoBehaviour Methods

    private void Start()
    {
        _cinemachineNoise = _cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        _camera = Camera.main;
        _defaultBackgroundColor = _camera.backgroundColor;
    }

    private void Update()
    {
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

    #endregion


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

        SetCameraNoise(_currentNoiseAmplitude, _currentNoiseFrequency);
    }

    public void SetCameraEffects(int fov, float noiseAmplitude, float noiseFrequency, float transitionDuration = 0.25f)
    {
        if (_cinemachineNoise == null)
            return;

        _activeTweeners.ForEach((x) => x.Kill());
        _activeTweeners.Clear();

        SetCameraFOV(fov, transitionDuration);
        SetCameraNoise(noiseAmplitude, noiseFrequency, transitionDuration);
    }

    private void SetCameraFOV(int fov, float transitionDuration = 0.25f)
    {
        Tweener fovTween = DOTween.To
        (
            () => _cinemachineCamera.Lens.FieldOfView,
            x => _cinemachineCamera.Lens.FieldOfView = x,
            (int)fov,
            transitionDuration
        );

        _activeTweeners.Add(fovTween);
    }

    private void SetCameraNoise(float amplitude, float frequency, float transitionDuration = 0.25f)
    {
        if (_isShaking || _cinemachineNoise == null)
            return;

        if (_currentNoiseAmplitude != amplitude)
            _currentNoiseAmplitude = amplitude;

        if (_currentNoiseFrequency != frequency)
            _currentNoiseFrequency = frequency;

        Tweener amplitudeTween = DOTween.To
        (
            () => _cinemachineNoise.AmplitudeGain,
            x => _cinemachineNoise.AmplitudeGain = x,
            amplitude,
            transitionDuration
        );

        Tweener frequencyTweener = DOTween.To
        (
            () => _cinemachineNoise.FrequencyGain,
            x => _cinemachineNoise.FrequencyGain = x,
            frequency,
            transitionDuration
        );

        _activeTweeners.Add(amplitudeTween);
        _activeTweeners.Add(frequencyTweener);
    }


    public void SetBackgroundColor(Color color, float transitionDuration)
        => Camera.main.DOColor(color, transitionDuration);

    public void ResetBackgroundColor(float transitionDuration)
        => SetBackgroundColor(_defaultBackgroundColor, transitionDuration);

}
