using UnityEngine;

public class EnergyTrigger : MonoBehaviour
{
    private bool _isActive = false;

    public bool IsActive => _isActive;

    public void Activate()
    {
        if (_isActive)
            return;

        _isActive = true;
    }

    public void Deactivate()
    {
        if (!_isActive)
            return;

        _isActive = false;
    }

}
