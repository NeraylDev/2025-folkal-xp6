using UnityEngine;

public class LevelFlashbackOneState : LevelBaseState
{
    private PlayerManager _playerManager;
    private DialogueManager _dialogueManager;

    public LevelFlashbackOneState(LevelStateMachine levelStateMachine, LevelManager levelManager) : base(levelStateMachine, levelManager)
    {
        _playerManager = levelManager.GetPlayerManager();
        _dialogueManager = levelManager.GetDialogueManager;
    }

    public override void Enter()
    {
        DialogueData dialogueData = _dialogueManager.GetFlashbackDatabase.GetDialogueData("Flashback_1");
        _dialogueManager.StartDialogue(dialogueData, true);
    }

}
