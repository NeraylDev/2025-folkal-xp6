using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIEvents _uiEvents;
    [Space]
    [SerializeField] private UIFadingElement _fadingElement;

    public UIEvents GetEvents => _uiEvents;
    public UIFadingElement GetFadingElement => _fadingElement;

    public static UIManager instance;

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        // --- Subsystems ---
        _fadingElement.Initialize(this);
    }
}
