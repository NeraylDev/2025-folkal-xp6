using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{

    [SerializeField] private GameObject _dialogueBox;
    [Space]
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private float _timePerLetter = 0.1f;
    private NPCData _npcData;
    private DialogueData _dialogueData;
    private int _lineIndex;
    public bool runningDialogue;
    private bool _typingText;

    private float _timeToClick = 0.1f;
    private float _currentTimeToClick;

    public static Dialogue instance;

    public UnityEvent onStartDialogue;
    public UnityEvent onFinishDialogue;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        onStartDialogue.AddListener(ShowDialogueBox);
        onFinishDialogue.AddListener(HideDialogueBox);
    }

    private void Update()
    {
        if (runningDialogue)
            _currentTimeToClick += Time.deltaTime;

        if(_currentTimeToClick >= _timeToClick && Input.GetKeyUp(KeyCode.E))
        {
            if (!_typingText)
            {
                _lineIndex++;
                if (_lineIndex < _dialogueData.Length)
                {
                    UpdateText();
                }
                else
                {
                    onFinishDialogue.Invoke();
                }
            }
            else
            {
                StopAllCoroutines();
                _typingText = false;

                UpdateText(true);
            }

            _currentTimeToClick = 0;
        }
    }

    public void StartDialogue(DialogueData dialogueData, NPCData NPCData)
    {
        if (runningDialogue)
            return;

        _npcData = NPCData;
        _dialogueData = dialogueData;
        _lineIndex = 0;

        onStartDialogue.Invoke();
    }

    private void UpdateText(bool instantaneously = false)
    {
        DialogueData.DialogueLine dialogueLine = _dialogueData.GetLine(_lineIndex);

        if (instantaneously)
        {
            _dialogueText.text = dialogueLine.GetText;
        }
        else
        {
            _dialogueText.text = "";
            StartCoroutine(TypeText(dialogueLine.GetText));
        }
    }

    private void ShowDialogueBox()
    {
        _dialogueBox.SetActive(true);
        UpdateText();

        _currentTimeToClick = 0;
        runningDialogue = true;
    }

    private void HideDialogueBox()
    {
        _dialogueBox.SetActive(false);
        runningDialogue = false;
    }


    private IEnumerator TypeText(string text)
    {
        _typingText = true;

        foreach (char l in text)
        {
            _dialogueText.text += l;
            yield return new WaitForSeconds(_timePerLetter);
        }

        _typingText = false;
    }
}
