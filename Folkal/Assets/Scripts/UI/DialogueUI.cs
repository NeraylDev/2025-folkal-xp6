using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;

public class DialogueUI : DialogueSubsystem
{
    [Header("Speech Settings")]
    [SerializeField] protected GameObject _dialogueBox;
    [Space]
    [SerializeField] private TMP_Text _characterNameText;
    [SerializeField] protected TMP_Text _dialogueText;

    protected override void SetEvents(InputActionAsset actionAsset)
    {
        _dialogueManager.GetEvents.onDialogueStart += (DialogueData data)
            => UpdateCharacterName(data);
        _dialogueManager.GetEvents.onDialogueStart += (DialogueData data)
            => ShowDialogueBox();

        _dialogueManager.GetEvents.onUpdateDialogueLine += (string lineText)
            => UpdateText(lineText);

        _dialogueManager.GetEvents.onDialogueEnd += (DialogueData data)
            => HideDialogueBox();
    }

    private void UpdateCharacterName(DialogueData data)
    {
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

    protected virtual void ShowDialogueBox()
    {
        _dialogueBox.SetActive(true);
    }

    protected virtual void HideDialogueBox()
    {
        _dialogueBox.SetActive(false);
    }
}
