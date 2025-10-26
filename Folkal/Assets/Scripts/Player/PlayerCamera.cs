using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
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
    private float _timeToStopShaking;
    private float _currentTimeToStopShaking;

    private bool _isShaking;

    private CinemachineCamera _cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin _cinemachineNoise;

    public static PlayerCamera instance;
    
    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        _cinemachineCamera = GetComponent<CinemachineCamera>();
        _cinemachineNoise = _cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        PlayerController playerController = PlayerController.instance;
        if (playerController != null)
        {
            playerController.onStartRun.AddListener(() => SetCameraFOV(FOV.Running));
            playerController.onStartRun.AddListener(() => SetCameraNoise(Noise.Running));
            playerController.onStopRun.AddListener(() => SetCameraFOV(FOV.Default));
            playerController.onStopRun.AddListener(() => SetCameraNoise(Noise.Default));
        } 
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

    private void UpdateCameraRaycast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _interactableMask)
            && !PlayerHand.instance.IsHoldingThrowable)
        {
            HUDManager.instance.SetCrosshair(HUDManager.CrosshairType.Interaction);
        }
        else
        {
            HUDManager.instance.SetCrosshair(HUDManager.CrosshairType.Default);
        }
    }

    public void TryInteract()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _interactableMask))
        {
            if (hit.collider.TryGetComponent(out Throwable throwable))
            {
                PlayerHand.instance.SetHeldThrowable(throwable);
            }
            else if (hit.collider.TryGetComponent(out NPC npc))
            {
                npc.TryStartDialogue();
            }
            else if (hit.collider.TryGetComponent(out Sign sign))
            {
                sign.TryStartReading();
            }
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

    public void SetCameraFOV(FOV fov)
    {
        float transitionDuration = 0.25f;
        switch (fov)
        {
            case FOV.Throwing:
                transitionDuration = 1f;
                break;
        }

        DOTween.To
        (
            () => _cinemachineCamera.Lens.FieldOfView,
            x => _cinemachineCamera.Lens.FieldOfView = x,
            (int)fov,
            transitionDuration
        );
    }

    public void SetCameraNoise(Noise noise)
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

        DOTween.To
        (
            () => _cinemachineNoise.AmplitudeGain,
            x => _cinemachineNoise.AmplitudeGain = x,
            amplitudeGain,
            transitionDuration
        );

        DOTween.To
        (
            () => _cinemachineNoise.FrequencyGain,
            x => _cinemachineNoise.FrequencyGain = x,
            frequencyGain,
            transitionDuration
        );
    }

}
