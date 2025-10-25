using UnityEngine;

public class Bomb : Throwable
{
    [Header("Bomb Settings")]
    [SerializeField] private GameObject _explosionVFX;
    [SerializeField] private LayerMask _explosionMask;
    [SerializeField] private LayerMask _wallMask;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField][Min(0)] private int _explosionDamage;
    private bool _wasThrown;

    public override void OnThrown()
    {
        if (!_wasThrown)
            _wasThrown = true;
    }

    public override void OnCollide()
    {
        if (!_wasThrown)
            return;

        Collider[] colliderArray = Physics.OverlapSphere(transform.position, _explosionRadius, _explosionMask);

        if (colliderArray == null)
            return;

        foreach (Collider collider in colliderArray)
        {
            Vector3 raycastDirection = (collider.transform.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, raycastDirection, _explosionRadius, _wallMask))
                continue;

            Transform hittedTransform = collider.transform;
            if (hittedTransform != gameObject)
            {
                float distanceToHittedObject = Vector3.Distance(transform.position, hittedTransform.position);
                float distanceModifier = Mathf.Lerp(1f, 0f, Mathf.Clamp01(distanceToHittedObject / _explosionRadius));

                if (hittedTransform.TryGetComponent(out Destructible destructible))
                    destructible.ApplyDamage(Mathf.CeilToInt(_explosionDamage * distanceModifier));

                ApplyExplosionForce(hittedTransform, _explosionForce * distanceModifier);
            }
        }

        ActivateParticles();
        Destroy(gameObject);
    }

    private void ApplyExplosionForce(Transform hittedTransform, float force)
    {
        Vector3 forceDirection = (hittedTransform.position - transform.position).normalized;
        Vector3 finalForce = forceDirection * _explosionForce;

        if (hittedTransform.TryGetComponent(out Rigidbody hittedRigidbody))
            hittedRigidbody.AddForce(finalForce, ForceMode.Impulse);
    }

    private void ActivateParticles()
    {
        if (_explosionVFX == null)
            return;

        _explosionVFX.SetActive(true);
        _explosionVFX.transform.parent = null;
    }

}
