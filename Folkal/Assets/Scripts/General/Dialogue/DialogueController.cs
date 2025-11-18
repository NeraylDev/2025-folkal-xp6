using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueController : DialogueSubsystem
{
    private DialogueData _currentDialogueData;
    private int _lineIndex;

    private string _lineText;
    private float _timePerLetter = 0.02f;
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

    public void StartDialogue(DialogueData data)
    {
        _lineText = "";
        _lineIndex = -1;
        _currentDialogueData = data;

        _dialogueManager.SetIsExecutingDialogue(true);
        TryUpdateLine(_currentDialogueData);

        InputSystem.actions.FindAction("Interact").canceled += OnSkipDialogueLine;
        _dialogueManager.GetEvents.RaiseDialogueStart(_currentDialogueData);
    }

    private void OnSkipDialogueLine(InputAction.CallbackContext context)
        => TryUpdateLine(_currentDialogueData);

    private void TryUpdateLine(DialogueData data)
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

    private void StopDialogue(DialogueData data)
    {
        StopAllCoroutines();

        InputSystem.actions.FindAction("Interact").canceled -= OnSkipDialogueLine;
        _dialogueManager.SetIsExecutingDialogue(false);
        _dialogueManager.GetEvents.RaiseDialogueEnd(data);
    }

    public DialogueData.DialogueLine GetCurrentLine(DialogueData data)
        => data.GetLine(_lineIndex);

    private IEnumerator TypeText(string text)
    {
        _isTypingText = true;

        bool isTypingTag = false;
        string currentTag = "";

        foreach (char letter in text)
        {
            if (letter == '<')
            {
                isTypingTag = true;
                currentTag = "";
            }

            if (isTypingTag)
            {
                currentTag += letter;

                if (letter == '>')
                {
                    isTypingTag = false;
                    _lineText += currentTag;
                }

                continue;
            }

            _lineText += letter;
            _dialogueManager.GetEvents.RaiseUpdateDialogueLine(_lineText);
            
            yield return new WaitForSeconds(_timePerLetter);
        }

        _isTypingText = false;
    }
}
