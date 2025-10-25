using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC Settings")]
    [SerializeField] private NPCData _data;
    [SerializeField] private List<DialogueData> _dialogueList = new List<DialogueData>();
    private int _dialogueIndex = 0;
    private bool _allowInteraction = true;

    public NPCData GetData => _data;

    public void TryStartDialogue()
    {
        DialogueUI dialogueUI = DialogueUI.instance;
        if (dialogueUI == null || !_allowInteraction)
            return;

        if (!dialogueUI.IsExecutingSpeech)
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
