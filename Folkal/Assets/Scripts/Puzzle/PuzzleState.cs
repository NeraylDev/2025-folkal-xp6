using UnityEngine;

public abstract class PuzzleState
{

    private PuzzleMachine _puzzleMachine;
    protected PuzzleMachine GetPuzzleMachine => _puzzleMachine;

    public PuzzleState(PuzzleMachine puzzleMachine)
    {
        _puzzleMachine = puzzleMachine;
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();

}
