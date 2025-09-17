using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Transform _handPosition;
    [SerializeField] private float _handPositionSpeed;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _handPosition.position, Time.deltaTime * _handPositionSpeed);
    }

}
