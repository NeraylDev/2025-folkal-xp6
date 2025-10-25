using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private int _life;

    public void ApplyDamage(int damage)
    {
        _life -= damage;
        if (_life <= 0)
        {
            Destroy(gameObject);
            Debug.Log($"{name} foi destruído.");
        }
    }
}
