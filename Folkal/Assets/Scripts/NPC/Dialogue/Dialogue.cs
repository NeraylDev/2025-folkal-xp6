using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;

public class Dialogue : Speech<DialogueData>
{

    [Header("Dialogue Settings")]
    [SerializeField] private TMP_Text _npcNameText;
    private NPCData _npcData;
    private DialogueData _dialogueData;

    public static Dialogue instance;

    public UnityEvent onStartDialogue;
    public UnityEvent onFinishDialogue;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        onStartDialogue.AddListener(ShowSpeechBox);
    }

    private void Start()
    {
        PlayerMovement playerMovement = PlayerMovement.instance;
        if (playerMovement != null)
        {
            onStartDialogue.AddListener(() => playerMovement.SetCanMove(false));
            onFinishDialogue.AddListener(() => playerMovement.SetCanMove(true));
        }
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

        onStartDialogue.Invoke();
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

    protected override void HideSpeechBox()
    {
        base.HideSpeechBox();
        onFinishDialogue.Invoke();
    }

    protected override bool IsDialogueFinished()
        => !(lineIndex < _dialogueData.Length);

}
