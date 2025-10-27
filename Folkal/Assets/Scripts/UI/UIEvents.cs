using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/UIEvents")]
public class UIEvents : ScriptableObject
{
    public Action onSpeechStart;
    public Action onSpeechEnd;

    public void RaiseSpeechStart()
        => onSpeechStart?.Invoke();
    public void RaiseSpeechEnd()
        => onSpeechEnd?.Invoke();
}
