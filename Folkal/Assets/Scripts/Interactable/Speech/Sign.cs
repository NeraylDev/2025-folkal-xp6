using System.Collections;
using UnityEngine;

public class Sign : MonoBehaviour, IInteractable
{
    [SerializeField] private SignData _data;
    private bool _allowInteraction = true;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact(PlayerManager playerManager)
    {
        TryStartReading(playerManager);
    }

    public void TryStartReading(PlayerManager playerManager)
    {
        SignUI signUI = SignUI.instance;
        if (signUI == null || !_allowInteraction)
            return;

        if (!signUI.IsExecutingSpeech && playerManager.CanReadSign())
        {
            signUI.StartSpeech(_data);
            signUI.onFinishSpeech.AddListener(UpdateInteraction);
            _allowInteraction = false;
        }
    }

    private void UpdateInteraction()
    {
        StartCoroutine(ActivateInteraction());

        SignUI signUI = SignUI.instance;
        if (signUI == null)
            return;

        signUI.onFinishSpeech.RemoveListener(UpdateInteraction);
    }

    private IEnumerator ActivateInteraction()
    {
        yield return new WaitForSeconds(0.05f);
        _allowInteraction = true;
    }
}
