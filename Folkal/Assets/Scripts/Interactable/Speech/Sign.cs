using System.Collections;
using UnityEngine;

public class Sign : MonoBehaviour, IInteractable
{
    [SerializeField] private SignData _data;
    private bool _allowInteraction = true;

    public void Interact(PlayerInteraction playerInteraction)
    {
        TryStartReading();
    }

    public void TryStartReading()
    {
        SignUI signUI = SignUI.instance;
        if (signUI == null || !_allowInteraction)
            return;

        if (!signUI.IsExecutingSpeech)
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
