using System;
using UnityEngine;

public abstract class PuzzleMachine : MonoBehaviour
{
    protected bool _isFinished;

    public abstract bool VerifyFinishCondition();

    protected virtual void OnFinish()
    {
        if (_isFinished)
            return;

        _isFinished = true;
    }

}
