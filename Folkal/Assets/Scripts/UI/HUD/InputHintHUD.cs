using TMPro;
using UnityEngine;

public class InputHintHUD : MonoBehaviour
{
    [SerializeField] private UIEvents _uiEvents;
    [Space]
    [SerializeField] private GameObject _layoutObject;
    [SerializeField] private TMP_Text _inputActionTxt;

    private void Awake()
    {
        _uiEvents.onShowInputHint += Show;
        _uiEvents.onHideInputHint += Hide;
    }

    private void Start()
    {
        if (_layoutObject.activeInHierarchy)
            _layoutObject.SetActive(false);
    }

    private void OnDisable()
    {
        _uiEvents.onShowInputHint -= Show;
        _uiEvents.onHideInputHint -= Hide;
    }

    private void Show(string action, string input)
    {
        _layoutObject.SetActive(true);
        _inputActionTxt.text = $"<color=#FDE16A>({input})</color> {action}";
    }

    private void Hide()
    {
        _inputActionTxt.text = "";
        _layoutObject.SetActive(false);
    }
}
