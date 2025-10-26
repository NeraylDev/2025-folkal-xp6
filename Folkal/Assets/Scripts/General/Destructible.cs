using UnityEngine;

public class Destructible : MonoBehaviour
{
    public void ApplyDamage()
    {
        Destroy(gameObject);
    }
}
