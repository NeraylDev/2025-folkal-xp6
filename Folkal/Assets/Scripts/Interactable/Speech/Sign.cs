using System.Collections;
using UnityEngine;

public class Sign : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueData _data;
    private bool _allowInteraction = true;

    private DialogueManager _dialogueManager;

    private void Start()
    {
        if (LevelManager.instance != null)
            _dialogueManager = LevelManager.instance.GetDialogueManager;
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact(PlayerManager playerManager)
    {
        TryStartReading(playerManager);
    }

    private void TryStartReading(PlayerManager playerManager)
    {
        if (_dialogueManager == null || !_allowInteraction || _data == null)
            return;

        _dialogueManager.StartDialogue(_data);
        _dialogueManager.GetEvents.onDialogueEnd += UpdateInteraction;

        _allowInteraction = false;
    }

    private void UpdateInteraction(DialogueData data)
    {
        StartCoroutine(ActivateInteraction());
        _dialogueManager.GetEvents.onDialogueEnd -= UpdateInteraction;
    }

    private IEnumerator ActivateInteraction()
    {
        yield return new WaitForSeconds(0.05f);
        _allowInteraction = true;
    }
}
