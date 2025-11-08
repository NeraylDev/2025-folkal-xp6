using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelFlashbackOneState : LevelBaseState
{
    private DialogueManager _dialogueManager;
    private DialogueData _flashbackDialogue;

    private UIManager _uiManager;
    private PlayerManager _playerManager;

    private Action<DialogueData> _onFirstDialogueEnd;
    private Action<PlayerManager> _onFirstBreathingEnd;
    private Action<DialogueData> _onSecondDialogueEnd;
    private Action<PlayerManager> _onSecondBreathingEnd;

    private bool _isTryingToActivateDialogue;
    private float _dialogueDelay;
    private float _currentDialogueDelay;

    private bool _blockPlayerMove;


    public LevelFlashbackOneState(LevelStateMachine levelStateMachine, LevelManager levelManager) : base(levelStateMachine, levelManager)
    {
        _dialogueManager = levelManager.GetDialogueManager;

        _uiManager = levelManager.GetUIManager();
        _playerManager = levelManager.GetPlayerManager();
    }

    public override void Enter()
    {
        _blockPlayerMove = true;
        _playerManager.GetPlayerCamera.SetCameraEffects(65, 0.15f, 5f, 5f);

        _flashbackDialogue = _dialogueManager.GetFlashbackDatabase.GetDialogueData("Flashback_1");
        StartDialogue(1);
        _playerManager.GetEvents.RaiseEnterMentalDimension(_playerManager, MentalDimensionPresenceHandler.MentalDimensionMode.Corruption, 0.1f);

        _onFirstDialogueEnd = OnFirstDialogueEnd;
        _onFirstBreathingEnd = OnFirstBreathingEnd;
        _onSecondDialogueEnd = OnSecondDialogueEnd;
        _dialogueManager.GetEvents.onDialogueEnd += _onFirstDialogueEnd;
    }

    public override void Execute()
    {
        if (_playerManager.GetPlayerMovement.CanMove && _blockPlayerMove)
            _playerManager.GetPlayerMovement.SetCanMove(false);

        if (_isTryingToActivateDialogue)
        {
            _currentDialogueDelay += Time.deltaTime;
            if (_currentDialogueDelay >= _dialogueDelay)
            {
                _dialogueManager.StartDialogue(_flashbackDialogue);
                _isTryingToActivateDialogue = false;
            }
        }
    }

    private void OnFirstDialogueEnd(DialogueData data)
    {
        InputSystem.actions.FindAction("Breath").started += StartBreathing;
        _uiManager.GetFadingElement.FadeOut(7.5f);

        _playerManager.GetEvents.onBreathingStop += _onFirstBreathingEnd;
        _dialogueManager.GetEvents.onDialogueEnd -= _onFirstDialogueEnd;
    }

    private void OnFirstBreathingEnd(PlayerManager playerManager)
    {
        InputSystem.actions.FindAction("Breath").started -= StartBreathing;

        GetLevelManager.GetMentalDimensionPresenceHandler.ChangeMode(MentalDimensionPresenceHandler.MentalDimensionMode.Refletion);

        _flashbackDialogue = _dialogueManager.GetFlashbackDatabase.GetDialogueData("Flashback_1_1");
        StartDialogue(_playerManager.GetPlayerBreathing.GetBreathingOutDuration + 4f);

        _dialogueManager.GetEvents.onDialogueEnd += _onSecondDialogueEnd;
        _playerManager.GetEvents.onBreathingStop -= _onFirstBreathingEnd;
    }

    private void OnSecondDialogueEnd(DialogueData data)
    {
        _playerManager.GetEvents.RaiseExitMentalDimension(_playerManager, _playerManager.GetPlayerBreathing.GetBreathingOutDuration);
        Exit();

        _dialogueManager.GetEvents.onDialogueEnd -= _onSecondDialogueEnd;
    }


    public void StartBreathing(InputAction.CallbackContext context)
        => _playerManager.GetPlayerBreathing.SetIsBreathing(true);

    public override void Exit()
    {
        _blockPlayerMove = false;
        GetLevelStateMachine.SetState(null);
    }
    
    private void StartDialogue(float delay = 0)
    {
        _currentDialogueDelay = 0;
        _dialogueDelay = delay;

        _isTryingToActivateDialogue = true;
    }
}
