using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelFlashbackOneState : LevelBaseState
{
    private DialogueManager _dialogueManager;
    private DialogueData _flashbackDialogue;

    private PlayerManager _playerManager;

    private Action<DialogueData> _onFirstDialogueEnd;
    private Action<PlayerManager> _onFirstBreathingEnd;
    private Action<DialogueData> _onSecondDialogueEnd;
    private Action<PlayerManager> _onSecondBreathingEnd;

    private bool _blockPlayerMove;

    public LevelFlashbackOneState(LevelStateMachine levelStateMachine, LevelManager levelManager) : base(levelStateMachine, levelManager)
    {
        _dialogueManager = levelManager.GetDialogueManager;
        _playerManager = levelManager.GetPlayerManager();
    }

    public override void Enter()
    {
        _flashbackDialogue = _dialogueManager.GetFlashbackDatabase.GetDialogueData("Flashback_1");
        _dialogueManager.StartDialogue(_flashbackDialogue, true);

        _blockPlayerMove = true;

        _playerManager.GetEvents.RaiseEnterMentalDimension(_playerManager, 0.1f);
        _onFirstDialogueEnd = OnFirstDialogueEnd;
        _onFirstBreathingEnd = OnFirstBreathingEnd;
        _onSecondDialogueEnd = OnSecondDialogueEnd;
        _onSecondBreathingEnd = OnSecondBreathingEnd;

        _dialogueManager.GetEvents.onDialogueEnd += _onFirstDialogueEnd;
    }

    public override void Execute()
    {
        if (_playerManager.GetPlayerMovement.CanMove && _blockPlayerMove)
            _playerManager.GetPlayerMovement.SetCanMove(false);
    }

    private void OnFirstDialogueEnd(DialogueData data)
    {
        InputSystem.actions.FindAction("Breath").started += (InputAction.CallbackContext context)
            => _playerManager.GetPlayerBreathing.SetIsBreathing(true);

        _dialogueManager.GetEvents.onDialogueEnd -= _onFirstDialogueEnd;
        _playerManager.GetEvents.onBreathingStop += _onFirstBreathingEnd;
    }

    private void OnFirstBreathingEnd(PlayerManager playerManager)
    {
        _flashbackDialogue = _dialogueManager.GetFlashbackDatabase.GetDialogueData("Flashback_1_1");
        _dialogueManager.StartDialogue(_flashbackDialogue, true);

        _playerManager.GetEvents.onBreathingStop -= _onFirstBreathingEnd;
        _dialogueManager.GetEvents.onDialogueEnd += _onSecondDialogueEnd;
    }

    private void OnSecondDialogueEnd(DialogueData data)
    {
        InputSystem.actions.FindAction("Breath").started += (InputAction.CallbackContext context)
            => _playerManager.GetPlayerBreathing.SetIsBreathing(true);

        _dialogueManager.GetEvents.onDialogueEnd -= _onSecondDialogueEnd;
        _playerManager.GetEvents.onBreathingStop += _onSecondBreathingEnd;
    }

    private void OnSecondBreathingEnd(PlayerManager playerManager)
    {
        _playerManager.GetEvents.RaiseExitMentalDimension(playerManager, playerManager.GetPlayerBreathing.GetBreathingOutDuration);
        Exit();

        _playerManager.GetEvents.onBreathingStop -= _onSecondBreathingEnd;
    }

    public override void Exit()
    {
        _blockPlayerMove = false;

        _playerManager.GetPlayerMovement.SetCanMove(true);
        GetLevelStateMachine.SetState(null);
    }
}
