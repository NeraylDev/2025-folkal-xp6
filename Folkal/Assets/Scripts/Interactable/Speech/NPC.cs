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

    private DialogueManager _dialogueManager;

    private void Start()
    {
        LevelManager levelManager = LevelManager.instance;
        if (levelManager != null)
            _dialogueManager = levelManager.GetDialogueManager;
    }

    public bool CanInteract()
    {
        return _allowInteraction;
    }

    public void Interact(PlayerManager playerManager)
    {
        TryStartDialogue(playerManager);
    }

    public void TryStartDialogue(PlayerManager playerManager)
    {
        if (!_dialogueManager.IsExecutingDialogue && playerManager.CanStartDialogue())
        {
            _dialogueManager.StartDialogue(_dialogueList[_dialogueIndex]);
            _dialogueManager.GetEvents.onDialogueEnd += (DialogueData data)
                => UpdateDialogueIndex();

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

        if (_dialogueManager != null)
        {
            _dialogueManager.GetEvents.onDialogueEnd -= (DialogueData data)
                => UpdateDialogueIndex();
        }
    }

    private IEnumerator ActivateInteraction()
    {
        yield return new WaitForSeconds(0.05f);
        _allowInteraction = true;
    }

}
