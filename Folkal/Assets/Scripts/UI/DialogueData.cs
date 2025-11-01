using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/DialogueData")]
public class DialogueData : ScriptableObject
{
    public enum Character
    {
        Geeko,
        Ma,
        Desconhecido
    }


    [SerializeField] private Character _character;
    [SerializeField] protected List<DialogueLine> _lineList;

    [HideInInspector] public string GetCharacterName => Enum.GetName(typeof(Character), _character);
    public int Length => _lineList.Count;

    public DialogueLine GetLine(int index)
    {
        if (_lineList.Count >= index + 1)
            return _lineList[index];
        return default;
    }

    [Serializable]
    public class DialogueLine
    {
        [SerializeField][TextArea] protected string text;

        public string GetText => text;
    }

    [Serializable]
    public class DialogueOption
    {
        [SerializeField] private string _text;
        [SerializeField] private DialogueData _nextDialogue;

        public string GetText => _text;
        public DialogueData GetNextDialogue => _nextDialogue;
    }
}