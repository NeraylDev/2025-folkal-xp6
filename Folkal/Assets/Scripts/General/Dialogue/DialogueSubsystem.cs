using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueSubsystem : MonoBehaviour
{
    protected DialogueManager _dialogueManager;

    public virtual void Initialize(DialogueManager dialogueManager, InputActionAsset actionAsset = null)
    {
        if (_dialogueManager != null)
            return;

        _dialogueManager = dialogueManager;

        if (actionAsset != null)
            SetEvents(actionAsset);
    }

    protected virtual void SetEvents(InputActionAsset actionAsset) { }
}
