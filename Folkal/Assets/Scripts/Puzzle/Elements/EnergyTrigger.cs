using System;
using UnityEngine;
using UnityEngine.Events;

public class EnergyTrigger : MonoBehaviour
{
    private bool _isActive = false;
    private bool _forceActivation = false;

    public bool IsActive => _isActive;


    private MaterialPropertyBlock _materialPropertyBlock;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    public void Activate()
    {
        if (_isActive || _meshRenderer == null)
            return;

        _materialPropertyBlock.Clear();
        _materialPropertyBlock.SetFloat("_EnergyTriggerIsActive", 1);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);    

        _isActive = true;
    }

    public void Deactivate()
    {
        if (!_isActive || _meshRenderer == null || _forceActivation)
            return;

        _materialPropertyBlock.Clear();
        _materialPropertyBlock.SetFloat("_EnergyTriggerIsActive", 0);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);        

        _isActive = false;
    }

    public void ForceActivation()
    {
        _forceActivation = true;
        Activate();
    }

}
