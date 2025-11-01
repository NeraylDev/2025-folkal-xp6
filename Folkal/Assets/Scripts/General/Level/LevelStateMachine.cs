using UnityEngine;

public class LevelStateMachine : StateMachine
{
    protected override void InitializeStateDictionary(IState initialState)
    {
        LevelBaseState state = initialState as LevelBaseState;
    }

    public LevelBaseState GetLevelState(string state)
    {
        IState temp = GetState(state);
        if (temp == null)
            return null;

        return temp as LevelBaseState;
    }
}
