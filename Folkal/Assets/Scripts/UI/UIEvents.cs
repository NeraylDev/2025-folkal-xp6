using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/UIEvents")]
public class UIEvents : ScriptableObject
{
    public Action<float> onFadeInToBlack;
    public Action<float> onFadeOutToBlack;
    public Action onFinishFadeInToBlack;
    public Action onFinishFadeOutToBlack;

    public Action<string, string> onShowInputHint;
    public Action onHideInputHint;

    public void RaiseFadeInToBlack(float duration)
        => onFadeInToBlack.Invoke(duration);

    public void RaiseFadeOutToBlack(float duration)
        => onFadeOutToBlack.Invoke(duration);

    public void RaiseFinishFadeInToBlack()
        => onFinishFadeInToBlack.Invoke();

    public void RaiseFinishFadeOutToBlack()
        => onFinishFadeOutToBlack.Invoke();

    public void RaiseShowInputHint(string action, string input)
        => onShowInputHint.Invoke(action, input);

    public void RaiseHideInputHint()
        => onHideInputHint.Invoke();
}
