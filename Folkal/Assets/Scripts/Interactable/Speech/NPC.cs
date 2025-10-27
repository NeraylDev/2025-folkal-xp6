using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("NPC Settings")]
    [SerializeField] private NPCData _data;
    [SerializeField] private List<DialogueData> _dialogueList = new List<DialogueData>();
    private int _dialogueIndex = 0;
    private bool _allowInteraction = true;

    public NPCData GetData => _data;

    public void Interact(PlayerInteraction playerInteraction)
    {
        TryStartDialogue();
    }

    public void TryStartDialogue()
    {
        PlayerManager playerController = PlayerManager.instance;
        DialogueUI dialogueUI = DialogueUI.instance;
        if (dialogueUI == null || playerController == null || !_allowInteraction)
            return;

        if (!dialogueUI.IsExecutingSpeech && !playerController.GetPlayerThrowing.IsLoadingThrow)
        {
            dialogueUI.StartSpeech(_dialogueList[_dialogueIndex], _data);
            dialogueUI.onFinishSpeech.AddListener(UpdateDialogueIndex);
            _allowInteraction = false;
        }
    }

    public void UpdateDialogueIndex()
    {
        if (_dialogueIndex + 1 < _dialogueList.Count)
        {
            _dialogueIndex++;
        }

        StartCoroutine(ActivateInteraction());

        DialogueUI dialogueUI = DialogueUI.instance;
        if (dialogueUI == null)
            return;

        dialogueUI.onFinishSpeech.RemoveListener(UpdateDialogueIndex);
    }

    private IEnumerator ActivateInteraction()
    {
        yield return new WaitForSeconds(0.05f);
        _allowInteraction = true;
    }

}
