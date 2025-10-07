using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpringJoint))]
public class EnergyFruit : Throwable
{
    [SerializeField] private float _maxFruitsDistance;
    private EnergyFruit _previousSibling;
    private EnergyFruit _nextSibling;
    private Vector3 _defaultPosition;
    private bool _wasThrown;
    private bool _isActive;

    private EnergyFruitLine _line;
    private SpringJoint _lineJoint;


    public bool IsActive => _isActive;

    public EnergyFruit GetPreviousSibling => _previousSibling;
    public EnergyFruit GetNextSibling => _nextSibling;


    #region MonoBehaviour Methods

    protected override void Awake()
    {
        base.Awake();

        _line = GetComponentInChildren<EnergyFruitLine>();
        _lineJoint = GetComponent<SpringJoint>();
        _defaultPosition = transform.position;
    }

    private void Start()
    {
        if (_lineJoint.connectedBody == null)
        {
            if (_nextSibling != null)
                _lineJoint.connectedBody = _nextSibling.GetComponent<Rigidbody>();
        }
    }

    #endregion


    public override void OnThrown()
    {
        base.OnThrown();

        if (!_wasThrown)
            _wasThrown = true;
    }

    public void SetPreviousSibling(EnergyFruit sibling)
    {
        if (_previousSibling != null)
            return;

        _previousSibling = sibling;
    }

    public void SetNextSibling(EnergyFruit sibling)
    {
        if (_nextSibling != null)
            return;
        
        _nextSibling = sibling;
    }


    public void ResetPosition(float movementDuration = 1f)
    {
        transform.DOMove(_defaultPosition, movementDuration);
        EnableRigidbody();

        _wasThrown = false;
        _isActive = false;
    }


    public override void OnCollide()
    {
        if (_wasThrown)
        {
            _isActive = true;
            DisableRigidbody();

            EnergyTree energyTree = _root as EnergyTree;
            if (energyTree.isAllFruitsActive())
                energyTree.ResetFruitsPosition();
        }
    }
}
