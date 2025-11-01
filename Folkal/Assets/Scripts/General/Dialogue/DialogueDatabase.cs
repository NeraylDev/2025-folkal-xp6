using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Database/DialogueDatabase")]
public class DialogueDatabase : ScriptableObject
{
    [SerializeField] private List<DialogueData> _dialogueDataList = new List<DialogueData>();

    private Dictionary<string, DialogueData> _dialogueDataDictionary = new Dictionary<string, DialogueData>();

    private void OnEnable()
    {
        foreach(DialogueData data in _dialogueDataList)
        {
            if (data != null && _dialogueDataDictionary.ContainsKey(data.name) == false)
                _dialogueDataDictionary.Add(data.name, data);
        }
    }

    public DialogueData GetDialogueData(string name)
    {
        _dialogueDataDictionary.TryGetValue(name, out DialogueData data);
        return data;
    }
}
