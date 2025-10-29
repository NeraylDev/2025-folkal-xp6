using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public enum CrosshairType
    {
        Default,
        Interaction
    }

    [SerializeField] private PlayerEvents _playerEvents;

    [Header("Prototype")]
    [SerializeField] private GameObject _finishedUI;

    [Header("Crosshair Settings")]
    [SerializeField] private GameObject _defaultCrosshair;
    [SerializeField] private GameObject _interactionCrosshair;
    private CrosshairType _currentCrosshair;

    public static HUDManager instance;

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(instance);
        instance = this;

        _currentCrosshair = CrosshairType.Default;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        _playerEvents.onStartPointingAtInteractable += (IInteractable interactable)
            => UpdateCrosshair(interactable);
        _playerEvents.onStopPointingAtInteractable += ()
            => UpdateCrosshair();
    }

    public void UpdateCrosshair(IInteractable interactable = null)
    {
        if (interactable == null)
        {
            SetCrosshair(CrosshairType.Default);
            return;
        }

        if (interactable.CanInteract())
        {
            SetCrosshair(CrosshairType.Interaction);
        }
        else
        {
            SetCrosshair(CrosshairType.Default);
        }
    }

    public void SetCrosshair(CrosshairType type)
    {
        if (type == _currentCrosshair)
            return;

        switch (type)
        {
            case CrosshairType.Default:
                _defaultCrosshair.SetActive(true);
                _interactionCrosshair.SetActive(false);
                break;
            case CrosshairType.Interaction:
                _defaultCrosshair.SetActive(false);
                _interactionCrosshair.SetActive(true);
                break;
        }

        _currentCrosshair = type;
    }
    
    public void ActivateFinishUI()
    {
        _finishedUI.SetActive(true);
    }

}
