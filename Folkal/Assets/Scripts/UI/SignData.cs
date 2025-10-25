using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SignData")]
public class SignData : ScriptableObject
{
    [SerializeField] protected List<SignLine> _lineList;

    public int Length => _lineList.Count;

    public SignLine GetLine(int index)
    {
        if (_lineList.Count >= index + 1)
            return _lineList[index];
        return default;
    }

    [Serializable]
    public class SignLine
    {
        [SerializeField][TextArea] protected string text;

        public string GetText => text;
    }
}
