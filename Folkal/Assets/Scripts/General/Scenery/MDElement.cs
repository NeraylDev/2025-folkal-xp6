using UnityEngine;

public class MDElement : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleArray;
    private bool _isActive;

    public bool IsActive => _isActive;

    public void Activate(Vector3 position)
    {
        transform.position = position;
        ActivateParticles();

        _isActive = true;
    }

    public void Deactivate()
    {
        DeactivateParticles();

        _isActive = false;
    }

    private void ActivateParticles()
    {
        foreach (ParticleSystem particle in _particleArray)
        {
            var emission = particle.emission;
            emission.enabled = true;
            particle.Play();
        }
    }

    private void DeactivateParticles()
    {
        foreach (ParticleSystem particle in _particleArray)
        {
            var emission = particle.emission;
            emission.enabled = false;
        }
    }
}
