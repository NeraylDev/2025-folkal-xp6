using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private float _defaultMoveSpeed;

    public float SetMoveSpeed { set { _moveSpeed = value; } }

    private PlayerController _playerController;

    private Rigidbody _rigidBody;

    public static PlayerMovement instance;

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        _rigidBody = GetComponent<Rigidbody>();
        _defaultMoveSpeed = _moveSpeed;
    }

    private void Start()
    {
        _playerController = PlayerController.instance;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        Vector3 finalDirection = (transform.right * _playerController.GetMoveDirection.x
                            + transform.forward * _playerController.GetMoveDirection.y).normalized;

        Vector3 finalVelocity = finalDirection * _moveSpeed * Time.fixedDeltaTime;
        _rigidBody.velocity = finalVelocity;
    }

    public float ResetMoveSpeed() => _moveSpeed = _defaultMoveSpeed;

    public void RotateY(float delta)
    {
        transform.Rotate(Vector3.up, delta * _playerController.GetMouseSensibility.x * Time.deltaTime);
    }

}
