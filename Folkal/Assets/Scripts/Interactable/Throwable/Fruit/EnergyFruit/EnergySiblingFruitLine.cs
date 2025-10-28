using System.Collections.Generic;
using UnityEngine;

public class EnergySiblingFruitLine : LineManager
{
    private EnergyFruit _fruit;

    private PlayerManager _playerManager;

    // Lista dos EnergyTriggers atingidos por RayCast no frame anterior

    #region MonoBehaviour Methods

    private void Awake()
    {
        _fruit = GetComponent<EnergyFruit>();
    }

    private void Start()
    {
        _playerManager = PlayerManager.instance;
    }

    private void Update()
    {
        if (_fruit.GetNextSibling == null)
            return;

        UpdateLinePosition(transform.position, _fruit.GetNextSibling.transform.position);
    }

    #endregion


    protected override void CalculateLineLength()
    {
        Vector3 previousSiblingPosition = _fruit.GetPreviousSibling.transform.position;
        Vector3 nextSiblingPosition = _fruit.GetNextSibling.transform.position;

        float distanceToPreviousSibling = Vector3.Distance(transform.position, previousSiblingPosition);
        float distanceToNextSibling = Vector3.Distance(transform.position, nextSiblingPosition);
        float greaterDistanceToSibling = distanceToPreviousSibling > distanceToNextSibling ? distanceToPreviousSibling : distanceToNextSibling;

        if (_playerManager.GetPlayerHand.IsHoldingThrowable && _playerManager.GetPlayerHand.GetHeldThrowable == _fruit)
        {
            // Modifica velocidade do Player de acordo com a distância até SiblingFruit
            float speedModifier = Mathf.Lerp(1, 0.25f, distanceToNextSibling / _maxLineLength);

            // Define o uso do modificador de acordo com a direção que o Player está se movendo
            Vector3 fruitToSiblingDirection = nextSiblingPosition - transform.position;
            Vector3 playerMovementDirection = _playerManager.GetPlayerMovement.GetMoveDirection;
            float directionModifier = Mathf.Clamp01(Vector3.Dot(playerMovementDirection, fruitToSiblingDirection));

            _playerManager.GetPlayerMovement.SetMoveSpeedModifier(Mathf.Lerp(speedModifier, 1, directionModifier));

            if (greaterDistanceToSibling > _maxLineLength)
                OnReachMaxLength();
        }
    }

    protected override void OnReachMaxLength()
    {
        _playerManager.GetPlayerHand.RemoveHeldThrowable();
        _playerManager.GetPlayerMovement.ResetMoveSpeedModifier();
    }

}
