using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObserverPlatform : MonoBehaviour
{
    [SerializeField] private PlayerEvents _playerEvents;
    [Space]
    [SerializeField] private List<Observer> _observerList = new List<Observer>();
    private PlayerManager _playerManager;
    private bool _isPlayerOnPlatform;
    private bool _isActive;

    private void Awake()
    {
        _playerEvents.onBreathingStop += (PlayerManager playerManager)
            => TryActivate(playerManager);
    }

    private void TryActivate(PlayerManager playerManager)
    {
        if (_playerManager == null || !_isPlayerOnPlatform)
            return;

        _playerEvents.RaiseEnterMentalDimension(playerManager);
        ActivateObservers();
    }

    private void ActivateObservers()
    {
        if (_observerList.Count <= 0)
            return;

        _observerList.ForEach((x) => x.Activate(_playerManager));
        _isActive = true;
    }

    private void DeactivateObservers()
    {
        _playerEvents.RaiseExitMentalDimension(_playerManager);

        _observerList.ForEach((x) => x.Deactivate());
        _isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (playerManager == null)
                return;

            _playerManager = playerManager;
            _isPlayerOnPlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (playerManager == null)
                return;

            if (_isActive)
                DeactivateObservers();

            _playerManager = null;
            _isPlayerOnPlatform = false;
        }
    }

}
