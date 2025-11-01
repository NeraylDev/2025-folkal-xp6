using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueController : DialogueSubsystem
{
    private DialogueData _currentDialogueData;
    private int _lineIndex;

    private string _lineText;
    private float _timePerLetter = 0.015f;
    private bool _isTypingText;

    private float _timeToClick = 0.1f;
    private float _currentTimeToClick;

    public string GetLineText => _lineText;

    private void Awake()
    {
        _currentTimeToClick = _timeToClick;
    }

    private void Update()
    {
        if (_dialogueManager.IsExecutingDialogue && _currentTimeToClick < _timeToClick)
            _currentTimeToClick += Time.deltaTime;
    }

    protected override void SetEvents(InputActionAsset actionAsset)
    {
        InputSystem.actions.FindAction("Interact").canceled += (InputAction.CallbackContext context)
            => TryUpdateLine(_currentDialogueData);
    }

    public void StartDialogue(DialogueData data, bool isLevelEvent = false)
    {
        _lineText = "";
        _lineIndex = -1;
        _currentDialogueData = data;

        _dialogueManager.SetIsExecutingDialogue(true);
        if (isLevelEvent)
            TryUpdateLine(_currentDialogueData);

        _dialogueManager.GetEvents.RaiseDialogueStart(_currentDialogueData);
    }

    public void TryUpdateLine(DialogueData data)
    {
        if (_dialogueManager.IsExecutingDialogue == false || _currentTimeToClick < _timeToClick)
            return;

        if (_isTypingText)
        {
            StopAllCoroutines();
            _lineText = GetCurrentLine(data).GetText;

            _isTypingText = false;
        }
        else
        {
            _lineIndex++;
            if (_lineIndex >= data.Length)
            {
                StopDialogue(data);
                return;
            }

            _lineText = "";
            StartCoroutine(TypeText(GetCurrentLine(data).GetText));
        }

        _dialogueManager.GetEvents.RaiseUpdateDialogueLine(_lineText);
    }

    public void StopDialogue(DialogueData data)
    {
        StopAllCoroutines();

        _dialogueManager.SetIsExecutingDialogue(false);
        _dialogueManager.GetEvents.RaiseDialogueEnd(data);
    }

    public DialogueData.DialogueLine GetCurrentLine(DialogueData data)
        => data.GetLine(_lineIndex);

    private IEnumerator TypeText(string text)
    {
        _isTypingText = true;
        char previousLetter;
        bool isTypingTag = false;

        foreach (char letter in text)
        {
            previousLetter = letter;
            _lineText += letter;

            if (isTypingTag)
            {
                if (previousLetter == '>')
                    isTypingTag = false;

                yield return new WaitForFixedUpdate();
            }

            isTypingTag = letter == '<';

            _dialogueManager.GetEvents.RaiseUpdateDialogueLine(_lineText);
            yield return new WaitForSeconds(_timePerLetter);
        }

        _isTypingText = false;
    }
}
