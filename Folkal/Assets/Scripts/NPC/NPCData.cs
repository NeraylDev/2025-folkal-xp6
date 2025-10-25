using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/NPCData")]
[Serializable]
public class NPCData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _name;

    public string GetID => _id;
    public string GetName => _name;
}
