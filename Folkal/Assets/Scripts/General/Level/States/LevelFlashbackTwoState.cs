using UnityEngine;

public class LevelFlashbackTwoState : LevelBaseState
{
    DialogueManager _dialogueManager;
    PlayerManager _playerManager;
    UIManager _uiManager;

    public LevelFlashbackTwoState(LevelStateMachine levelStateMachine, LevelManager levelManager) : base(levelStateMachine, levelManager)
    {
        _dialogueManager = levelManager.GetDialogueManager;
        _playerManager = levelManager.GetPlayerManager();
        _uiManager = levelManager.GetUIManager();
    }

    public override void Enter()
    {
        DialogueData flashbackData = _dialogueManager.GetFlashbackDatabase.GetDialogueData("Flashback_2");

        _dialogueManager.StartDialogue(flashbackData);
        _dialogueManager.GetEvents.onDialogueEnd += OnFlashbackEnd;

        _playerManager.GetPlayerMovement.SetCanMove(false);
        _uiManager.GetEvents.RaiseFadeInToBlack(1f);

        
    }

    private void OnFlashbackEnd(DialogueData data)
    {
        _dialogueManager.GetEvents.onDialogueEnd -= OnFlashbackEnd;

        GetLevelManager.LoadScene(0, 0.5f);
    }

}
