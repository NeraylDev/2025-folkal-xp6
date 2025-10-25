using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public enum FOV
    {
        Default = 65,
        Running = 70,
        Throwing = 62
    }

    public enum Shake
    {
        None,
        Default,
        Running
    }

    [Header("Interaction Settings")]
    [SerializeField] private float _interactionDistance;
    [SerializeField] private LayerMask _interactableMask;

    private CinemachineCamera _cinemachineCamera;

    public static PlayerCamera instance;
    
    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        _cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        PlayerController playerController = PlayerController.instance;
        if (playerController != null)
        {
            playerController.onStartRun.AddListener(() => SetCameraFOV(FOV.Running));
            playerController.onStartRun.AddListener(() => SetCameraShake(Shake.Running));
            playerController.onStopRun.AddListener(() => SetCameraFOV(FOV.Default));
            playerController.onStopRun.AddListener(() => SetCameraShake(Shake.Default));
        } 
    }

    private void Update()
    {
        UpdateCameraRaycast();
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

    public void SetCameraShake(Shake shake)
    {
        CinemachineBasicMultiChannelPerlin cameraNoise = _cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (cameraNoise == null)
            return;

        float amplitudeGain = 0;
        float frequencyGain = 0;
        float transitionDuration = 0.25f;
        switch (shake)
        {
            case Shake.Default:
                amplitudeGain = 0.4f;
                frequencyGain = 0.25f;
                break;

            case Shake.Running:
                amplitudeGain = 0.25f;
                frequencyGain = 3.5f;
                break;
        }

        DOTween.To
        (
            () => cameraNoise.AmplitudeGain,
            x => cameraNoise.AmplitudeGain = x,
            amplitudeGain,
            transitionDuration
        );

        DOTween.To
        (
            () => cameraNoise.FrequencyGain,
            x => cameraNoise.FrequencyGain = x,
            frequencyGain,
            transitionDuration
        );
    }

}
