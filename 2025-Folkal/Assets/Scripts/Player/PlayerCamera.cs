using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public static PlayerCamera instance;

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

}
