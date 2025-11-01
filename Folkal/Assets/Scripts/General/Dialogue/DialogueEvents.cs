using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/DialogueEvents")]
public class DialogueEvents : ScriptableObject
{
    public Action<DialogueData> onDialogueStart;
    public Action<string> onUpdateDialogueLine;
    public Action<DialogueData> onDialogueEnd;

    public void RaiseDialogueStart(DialogueData data)
        => onDialogueStart?.Invoke(data);

    public void RaiseUpdateDialogueLine(string lineText)
        => onUpdateDialogueLine?.Invoke(lineText);

    public void RaiseDialogueEnd(DialogueData data)
        => onDialogueEnd?.Invoke(data);
}
