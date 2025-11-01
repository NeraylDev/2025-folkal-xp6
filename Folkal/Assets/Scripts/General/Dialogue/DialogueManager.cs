using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueEvents _events;
    [Space]
    [SerializeField] private DialogueController _dialogueController;
    [SerializeField] private DialogueUI _dialogueUI;
    private bool _isExecutingDialogue;

    [Header("Database")]
    [SerializeField] private DialogueDatabase _flashbackDatabase;

    public DialogueEvents GetEvents => _events;
    public DialogueController GetDialogueController => _dialogueController;
    public DialogueUI GetDialogueUI => _dialogueUI;

    public DialogueDatabase GetFlashbackDatabase => _flashbackDatabase;

    public bool IsExecutingDialogue => _isExecutingDialogue;
    public void SetIsExecutingDialogue(bool value) => _isExecutingDialogue = value;


    private void Start()
    {
        InputActionAsset actionAsset = InputSystem.actions;
        _dialogueController.Initialize(this, actionAsset);
        _dialogueUI.Initialize(this, actionAsset);
    }

    public void StartDialogue(DialogueData data, bool isLevelEvent = false)
    {
        if (_isExecutingDialogue)
            return;

        _dialogueController.StartDialogue(data, isLevelEvent);
    }
}
