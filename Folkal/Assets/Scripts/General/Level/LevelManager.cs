using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private LevelStateMachine _levelStateMachine;

    private PlayerManager _playerManager;
    private DialogueManager _dialogueManager;

    private MentalDimensionPresenceHandler _mentalDimensionPresenceHandler;

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

    private void Start()
    {
        _levelStateMachine.Initialize(new LevelFlashbackOneState(_levelStateMachine, this));
    }

    public PlayerManager GetPlayerManager()
    {
        if (_playerManager == null)
            _playerManager = PlayerManager.instance;

        return _playerManager;
    }

    private void OnSceneLoaded()
    {
        _playerManager = PlayerManager.instance;
    }
}
