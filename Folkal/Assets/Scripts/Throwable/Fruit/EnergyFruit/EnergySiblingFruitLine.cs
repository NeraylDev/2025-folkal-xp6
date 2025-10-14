using System.Collections.Generic;
using UnityEngine;

public class EnergySiblingFruitLine : LineManager
{
    private EnergyFruit _fruit;

    private PlayerHand _playerHand;
    private PlayerMovement _playerMovement;

    // Lista dos EnergyTriggers atingidos por RayCast no frame anterior
    private List<EnergyTrigger> _previousCollidedTriggers = new List<EnergyTrigger>();

    #region MonoBehaviour Methods

    private void Awake()
    {
        _fruit = GetComponent<EnergyFruit>();
    }

    private void Start()
    {
        _playerHand = PlayerHand.instance;
        _playerMovement = PlayerMovement.instance;
    }

    private void Update()
    {
        if (_fruit.GetNextSibling == null)
            return;

        UpdateLinePosition(transform.position, _fruit.GetNextSibling.transform.position);
        FindEnergyTrigger();
    }

    #endregion


    protected override void CalculateLineLength()
    {
        Vector3 previousSiblingPosition = _fruit.GetPreviousSibling.transform.position;
        Vector3 nextSiblingPosition = _fruit.GetNextSibling.transform.position;

        float distanceToPreviousSibling = Vector3.Distance(transform.position, previousSiblingPosition);
        float distanceToNextSibling = Vector3.Distance(transform.position, nextSiblingPosition);
        float greaterDistanceToSibling = distanceToPreviousSibling > distanceToNextSibling ? distanceToPreviousSibling : distanceToNextSibling;

        if (_playerHand.IsHoldingThrowable && _playerHand.GetHeldThrowable == _fruit)
        {
            // Modifica velocidade do Player de acordo com a distância até SiblingFruit
            float speedModifier = Mathf.Lerp(1, 0.25f, distanceToNextSibling / _maxLineLength);

            // Define o uso do modificador de acordo com a direção que o Player está se movendo
            Vector3 fruitToSiblingDirection = nextSiblingPosition - transform.position;
            Vector3 playerMovementDirection = _playerMovement.GetMoveDirection;
            float directionModifier = Mathf.Clamp01(Vector3.Dot(playerMovementDirection, fruitToSiblingDirection));

            _playerMovement.SetMoveSpeedModifier(Mathf.Lerp(speedModifier, 1, directionModifier));

            if (greaterDistanceToSibling > _maxLineLength)
                OnReachMaxLength();
        }
    }

    protected override void OnReachMaxLength()
    {
        _playerHand.RemoveHeldThrowable();
        _playerMovement.ResetMoveSpeedModifier();
    }

    private void FindEnergyTrigger()
    {
        Vector3 siblingPosition = _fruit.GetNextSibling.transform.position;
        Vector3 directionToSibling = (siblingPosition - transform.position).normalized;
        float distanceToSibling = Vector3.Distance(transform.position, siblingPosition);

        List<EnergyTrigger> _currentCollidedTriggers = new List<EnergyTrigger>();

        RaycastHit[] hitArray = Physics.RaycastAll(transform.position, directionToSibling, distanceToSibling);
        foreach (RaycastHit hit in hitArray)
        {
            if (hit.collider.CompareTag("EnergyTrigger") && hit.collider.TryGetComponent(out EnergyTrigger trigger))
            {
                _currentCollidedTriggers.Add(trigger);

                //if (!_previousCollidedTriggers.Contains(trigger))
                OnEnergyTriggerEnter(trigger);
            }
        }

        foreach (EnergyTrigger oldTrigger in _previousCollidedTriggers)
        {
            if (!_currentCollidedTriggers.Contains(oldTrigger))
                OnEnergyTriggerExit(oldTrigger);
        }

        _previousCollidedTriggers.Clear();
        foreach (EnergyTrigger trigger in _currentCollidedTriggers)
            _previousCollidedTriggers.Add(trigger);
    }

    private void OnEnergyTriggerEnter(EnergyTrigger energyTrigger)
    {
       energyTrigger.Activate();
    }

    private void OnEnergyTriggerExit(EnergyTrigger energyTrigger)
    {
        energyTrigger.Deactivate();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Vector3 siblingPosition = _fruit.GetNextSibling.transform.position;
        Vector3 directionToSibling = (siblingPosition - transform.position).normalized;
        float distanceToSibling = Vector3.Distance(transform.position, siblingPosition);

        List<EnergyTrigger> _currentCollidedTriggers = new List<EnergyTrigger>();

        Gizmos.DrawLine(transform.position, directionToSibling * distanceToSibling);
    }

}
