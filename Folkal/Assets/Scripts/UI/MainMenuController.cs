using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private UIEvents _uiEvents;
    private InputActionAsset _inputActions;
    float _timeToStart = 1f;
    float _currentTimeToStart;
    bool _started;

    private void Awake()
    {
        _inputActions = InputSystem.actions;
    }

    private void Start()
    {
        _inputActions.FindAction("Breath").canceled += PlayGame;

        _uiEvents.RaiseFadeOutToBlack(1f);
    }

    private void Update()
    {
        if (_started)
        {
            _currentTimeToStart += Time.deltaTime;
            if (_currentTimeToStart >= _timeToStart)
            {
                SceneManager.LoadScene(1);
            }
        }
    }

    public void PlayGame(InputAction.CallbackContext context)
    {
        _uiEvents.RaiseFadeInToBlack(_timeToStart);
        _started = true;

        _inputActions.FindAction("Breath").canceled -= PlayGame;
    }
}
