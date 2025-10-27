using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerStates")]
public class PlayerEvents : ScriptableObject
{
    public Action<bool> onInteractionStateChanged;

    public void RaiseInteractionStateChanged(bool value)
        => onInteractionStateChanged.Invoke(value);
}
