using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private float _defaultMoveSpeed;

    public float SetMoveSpeed { set { _moveSpeed = value; } }

    private PlayerController _playerController;

    private Camera _playerCamera;
    private Rigidbody _rigidBody;

    public static PlayerMovement instance;


    #region MonoBehaviour Methods

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
        _playerCamera = Camera.main;
    }

    private void Update()
    {
        UpdateRotation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    #endregion


    public void Move()
    {
        Vector3 finalDirection = (transform.right * _playerController.GetMoveDirection.x
                            + transform.forward * _playerController.GetMoveDirection.y).normalized;

        Vector3 finalVelocity = finalDirection * _moveSpeed * Time.fixedDeltaTime;
        _rigidBody.linearVelocity = finalVelocity;
    }

    public float ResetMoveSpeed() => _moveSpeed = _defaultMoveSpeed;

    public void UpdateRotation()
    {
        Vector3 finalRotation = _playerCamera.transform.rotation.eulerAngles;
        finalRotation.x = 0;
        finalRotation.z = 0;

        transform.eulerAngles = finalRotation;
    }

}
