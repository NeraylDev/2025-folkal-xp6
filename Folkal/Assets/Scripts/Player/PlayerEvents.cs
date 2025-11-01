using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerEvents")]
public class PlayerEvents : ScriptableObject
{
    // --- Movement Events ---
    public event Action<PlayerManager> onWalkStart;
    public event Action<PlayerManager> onWalkStop;

    public event Action<PlayerManager> onRunStart;
    public event Action<PlayerManager> onRunStop;
    

    // --- Hand Events ---
    public event Action<PlayerManager> onPickUpThrowable;
    public event Action<PlayerManager> onDropThrowable;

    public event Action<PlayerManager> onThrowingStart;
    public event Action<PlayerManager> onThrow;


    // --- Mental Dimension Events ---
    public event Action<PlayerManager, float> onEnterMentalDimension;
    public event Action<PlayerManager, float> onExitMentalDimension;


    // --- Other Events ---
    public event Action<PlayerManager> onInteract;
    public event Action<IInteractable> onStartPointingAtInteractable;
    public event Action onStopPointingAtInteractable;
    
    public event Action<PlayerManager> onBreathingStart;
    public event Action<PlayerManager> onBreathingCanceled;
    public event Action<PlayerManager> onBreathingStop;

    
    public void RaiseWalkStart(PlayerManager playerManager)
        => onWalkStart?.Invoke(playerManager);

    public void RaiseWalkStop(PlayerManager playerManager)
        => onWalkStop?.Invoke(playerManager);


    public void RaiseRunStart(PlayerManager playerManager)
        => onRunStart?.Invoke(playerManager);

    public void RaiseRunStop(PlayerManager playerManager)
        => onRunStop?.Invoke(playerManager);




    public void RaisePickUpThrowable(PlayerManager playerManager)
        => onPickUpThrowable?.Invoke(playerManager);

    public void RaiseDropThrowable(PlayerManager playerManager)
        => onDropThrowable?.Invoke(playerManager);


    public void RaiseThrowingStart(PlayerManager playerManager)
        => onDropThrowable?.Invoke(playerManager);

    public void RaiseThrow(PlayerManager playerManager)
        => onThrow?.Invoke(playerManager);



    public void RaiseEnterMentalDimension(PlayerManager playerManager, float duration)
        => onEnterMentalDimension?.Invoke(playerManager, duration);
    public void RaiseExitMentalDimension(PlayerManager playerManager, float duration)
        => onExitMentalDimension?.Invoke(playerManager, duration);



    public void RaiseStartPointingAtInteractable(IInteractable interactable)
        => onStartPointingAtInteractable?.Invoke(interactable);

    public void RaiseStopPointingAtInteractable()
        => onStopPointingAtInteractable?.Invoke();

    public void RaiseInteract(PlayerManager playerManager)
        => onInteract?.Invoke(playerManager);


    public void RaiseBreathingStart(PlayerManager playerManager)
        => onBreathingStart?.Invoke(playerManager);

    public void RaiseBreathingCanceled(PlayerManager playerManager)
        => onBreathingCanceled?.Invoke(playerManager);

    public void RaiseBreathingStop(PlayerManager playerManager)
        => onBreathingStop?.Invoke(playerManager);
}
