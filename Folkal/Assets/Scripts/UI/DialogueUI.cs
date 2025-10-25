using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;

public class DialogueUI : Speech<DialogueData>
{

    [Header("Dialogue Settings")]
    [SerializeField] private TMP_Text _npcNameText;
    private NPCData _npcData;
    private DialogueData _dialogueData;

    public static DialogueUI instance;

    protected override void Awake()
    {
        base.Awake();

        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    public void StartSpeech(DialogueData dialogueData, NPCData npcData)
    {
        if (IsExecutingSpeech)
            return;

        _npcData = npcData;
        _npcNameText.text = _npcData.GetName;

        StartSpeech(dialogueData);
    }

    public override void StartSpeech(DialogueData data)
    {
        _dialogueData = data;
        lineIndex = 0;

        onStartSpeech.Invoke();
    }

    protected override void UpdateText(bool instantaneously = false)
    {
        DialogueData.DialogueLine dialogueLine = _dialogueData.GetLine(lineIndex);
        if (dialogueLine == null)
            return;

        if (instantaneously)
        {
            GetSpeechText.text = dialogueLine.GetText;
        }
        else
        {
            GetSpeechText.text = "";
            StartCoroutine(TypeText(dialogueLine.GetText));
        }
    }

    protected override bool IsDialogueFinished()
        => !(lineIndex < _dialogueData.Length);

}
