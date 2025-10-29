using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObserverPlatform : MonoBehaviour
{
    [SerializeField] private List<Observer> _observerList = new List<Observer>();
    private PlayerManager _playerManager;
    private bool _isPlayerOnPlatform;
    private bool _isActive;

    private void Update()
    {
        if (_playerManager == null)
            return;

        if (_isPlayerOnPlatform && _playerManager.GetPlayerBreathing.IsBreathing)
        {
            ActivateObservers();
        }
    }

    private void ActivateObservers()
    {
        _observerList.ForEach((x) => x.Activate());
        _isActive = true;
    }

    private void DeactivateObservers()
    {
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

            _playerManager = null;
            _isPlayerOnPlatform = false;
            
            if (_isActive)
                DeactivateObservers();
        }
    }

}
