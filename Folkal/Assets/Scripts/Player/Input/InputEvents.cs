using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/InputEvents")]
public class InputEvents : ScriptableObject
{
    public event Action onActionStart;
    public event Action onActionStop;

    public event Action onInteractStart;
    public event Action onInteractStop;
}
