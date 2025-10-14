using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnergyPuzzle : PuzzleMachine
{
    [Header("Energy Puzzle Settings")]
    [SerializeField] private List<EnergyTrigger> _energyTriggers;

    private void Awake()
    {
        LoadTriggerList(GetComponentsInChildren<EnergyTrigger>());
    }

    private void Update()
    {
        if (VerifyFinishCondition())
        {
            OnFinish();
        }
    }

    private void LoadTriggerList(EnergyTrigger[] list)
    {
        if (list == null)
            return;

        foreach (EnergyTrigger trigger in list)
        {
            _energyTriggers.Add(trigger);
        }
    }

    public override bool VerifyFinishCondition()
    {
        // Verify if all the triggers are active
        foreach (EnergyTrigger trigger in _energyTriggers)
        {
            if (!trigger.IsActive)
                return false;
        }

        return true;
    }

    protected override void OnFinish()
    {
        base.OnFinish();
        HUDManager.instance.ActivateFinishUI();
        _energyTriggers.ForEach((x) => x.ForceActivation());
    }

}
