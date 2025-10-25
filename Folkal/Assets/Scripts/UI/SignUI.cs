using UnityEngine;
using UnityEngine.Events;

public class SignUI : Speech<SignData>
{
    private SignData _signData;

    public static SignUI instance;

    protected override void Awake()
    {
        base.Awake();

        // --- Singleton ---
        if (instance != null)
            Destroy(instance);
        instance = this;
    }

    public override void StartSpeech(SignData data)
    {
        _signData = data;
        lineIndex = 0;

        onStartSpeech.Invoke();
    }

    protected override void UpdateText(bool instantaneously = false)
    {
        SignData.SignLine signLine = _signData.GetLine(lineIndex);
        if (signLine == null)
            return;

        if (instantaneously)
        {
            GetSpeechText.text = signLine.GetText;
        }
        else
        {
            GetSpeechText.text = "";
            StartCoroutine(TypeText(signLine.GetText));
        }
    }

    protected override bool IsDialogueFinished()
        => !(lineIndex < _signData.Length);
}
