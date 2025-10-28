using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObserverPlatform : MonoBehaviour
{
    [SerializeField] private List<Observer> _observerList = new List<Observer>();
    private PlayerManager _playerManager;
    private bool _isPlayerOnPlatform = false;

    private void ActivateObservers()
    {
        _observerList.ForEach((x) => x.Activate());
    }

    private void DeactivateObservers()
    {
        _observerList.ForEach((x) => x.Deactivate());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (playerManager == null)
                return;

            ActivateObservers();
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
            
            DeactivateObservers();
            _playerManager = playerManager;
            _isPlayerOnPlatform = false;
        }
    }

}
