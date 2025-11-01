using UnityEngine;

public class LevelBaseState : IState
{
    private LevelManager _levelManager;
    private LevelStateMachine _levelStateMachine;

    public LevelManager GetLevelManager => _levelManager;
    public LevelStateMachine GetLevelStateMachine => _levelStateMachine;

    public LevelBaseState(LevelStateMachine levelStateMachine, LevelManager levelManager)
    {
        _levelManager = levelManager;
        _levelStateMachine = levelStateMachine;
    }

    public virtual void Enter() { }

    public virtual void Execute() { }

    public virtual void Exit() { }

    public virtual void FixedExecute() { }

    public virtual void TryExit() { }
}
