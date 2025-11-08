using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private LevelStateMachine _levelStateMachine;

    private UIManager _uiManager;
    private PlayerManager _playerManager;
    private DialogueManager _dialogueManager;

    private MentalDimensionPresenceHandler _mentalDimensionPresenceHandler;

    private float _timeToRunMachine = 0.05f;
    private float _currentTimeToRunMachine;
    private bool _isMachineRunning;

    public DialogueManager GetDialogueManager => _dialogueManager;
    public MentalDimensionPresenceHandler GetMentalDimensionPresenceHandler
        => _mentalDimensionPresenceHandler;


    public static LevelManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode)
            => OnSceneLoaded();

        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        // --- Children Getters ---
        _dialogueManager = GetComponentInChildren<DialogueManager>();
        _mentalDimensionPresenceHandler = GetComponentInChildren<MentalDimensionPresenceHandler>();

        // --- State Machine ---
        _levelStateMachine = new LevelStateMachine();
    }

    private void Update()
    {
        if (_currentTimeToRunMachine < _timeToRunMachine)
        {
            _currentTimeToRunMachine += Time.deltaTime;
        }
        else if (_isMachineRunning == false)
        {
            //_levelStateMachine.Initialize(new LevelFlashbackOneState(_levelStateMachine, this));
            _isMachineRunning = true;
        }

        _levelStateMachine.Execute();
    }

    private void FixedUpdate()
    {
        _levelStateMachine.FixedExecute();
    }

    public UIManager GetUIManager()
    {
        if (_uiManager == null)
            _uiManager = UIManager.instance;

        return _uiManager;
    }

    public PlayerManager GetPlayerManager()
    {
        if (_playerManager == null)
            _playerManager = PlayerManager.instance;

        return _playerManager;
    }

    private void OnSceneLoaded()
    {
        _uiManager = UIManager.instance;
        _playerManager = PlayerManager.instance;
    }
}
