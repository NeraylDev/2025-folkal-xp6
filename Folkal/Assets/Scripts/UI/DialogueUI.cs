using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;

public class DialogueUI : DialogueSubsystem
{
    [Header("Speech Settings")]
    [SerializeField] private GameObject _dialogueBox;
    [Space]
    [SerializeField] private TMP_Text _characterNameText;
    [SerializeField] private TMP_Text _dialogueText;

    protected override void SetEvents(InputActionAsset actionAsset)
    {
        _dialogueManager.GetEvents.onDialogueStart += OnDialogueStart;
        _dialogueManager.GetEvents.onUpdateDialogueLine += OnUpdateDialogueLine;
        _dialogueManager.GetEvents.onDialogueEnd += OnDialogueEnd;
    }

    private void OnDisable()
    {
        _dialogueManager.GetEvents.onDialogueStart -= OnDialogueStart;
        _dialogueManager.GetEvents.onUpdateDialogueLine -= OnUpdateDialogueLine;
        _dialogueManager.GetEvents.onDialogueEnd -= OnDialogueEnd;
    }

    private void OnDialogueStart(DialogueData data)
    {
        UpdateCharacterName(data);
        ShowDialogueBox();
    }

    private void OnUpdateDialogueLine(string lineText)
    {
        UpdateText(lineText);
    }

    private void OnDialogueEnd(DialogueData data)
    {
        HideDialogueBox();
    }


    private void UpdateCharacterName(DialogueData data)
    {
        if (data.GetCharacterName == "None")
        {
            _characterNameText.enabled = false;
            return;
        }
        else
        {
            _characterNameText.enabled = true;
        }

        Color initialNameColor = _characterNameText.color;
        initialNameColor.a = 0;

        Color finalNameColor = initialNameColor;
        finalNameColor.a = 1;

        _characterNameText.color = initialNameColor;
        _characterNameText.DOColor(finalNameColor, 0.5f);
        _characterNameText.text = data.GetCharacterName;
    }

    private void UpdateText(string lineText)
    {
        _dialogueText.text = lineText;
    }

    private void ShowDialogueBox()
    {
        _dialogueBox.SetActive(true);
    }

    private void HideDialogueBox()
    {
        _dialogueBox.SetActive(false);
    }
}
