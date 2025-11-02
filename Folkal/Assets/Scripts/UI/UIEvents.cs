using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/UIEvents")]
public class UIEvents : ScriptableObject
{
    public Action<float> onFadeInToBlack;
    public Action<float> onFadeOutToBlack;

    public void RaiseFadeInToBlack(float duration)
        => onFadeInToBlack.Invoke(duration);

    public void RaiseFadeOutToBlack(float duration)
        => onFadeOutToBlack.Invoke(duration);
}
