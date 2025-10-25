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
        if (Dialogue.instance.IsExecutingSpeech || _allowInteraction == false)
            return;

        Dialogue.instance.StartSpeech(_dialogueList[_dialogueIndex], _data);
        Dialogue.instance.onFinishDialogue.AddListener(UpdateDialogueIndex);
        _allowInteraction = false;
    }

    public void UpdateDialogueIndex()
    {
        if (_dialogueIndex + 1 < _dialogueList.Count)
        {
            _dialogueIndex++;
        }

        Dialogue.instance.onFinishDialogue.RemoveListener(UpdateDialogueIndex);
        StartCoroutine(ActivateInteraction());
    }

    private IEnumerator ActivateInteraction()
    {
        yield return new WaitForSeconds(0.05f);
        _allowInteraction = true;
    }

}
