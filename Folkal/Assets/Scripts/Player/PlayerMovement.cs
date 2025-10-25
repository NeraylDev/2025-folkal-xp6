using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private float _defaultMoveSpeed;
    private float _moveSpeedModifier = 1f;
    private Vector3 _moveDirection;
    private bool _canMove;

private PlayerController _playerController;

    private Camera _playerCamera;
    private Rigidbody _rigidBody;

    public bool CanMove => _canMove;
    public Vector3 GetMoveDirection => _moveDirection;

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
        _canMove = true;
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
        if (!_canMove)
            return;

        _moveDirection = (transform.right * _playerController.GetMoveDirection.x
                            + transform.forward * _playerController.GetMoveDirection.y).normalized;

        Vector3 finalVelocity = _moveDirection * _moveSpeed * _moveSpeedModifier * Time.fixedDeltaTime;
        _rigidBody.linearVelocity = finalVelocity;
    }

    public void SetCanMove(bool value)
    {
        _canMove = value;
        if (!_canMove)
        {
            _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            _rigidBody.linearVelocity = Vector3.zero;
        }
        else
        {
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public void SetMoveSpeed(float speed) => _moveSpeed = speed;
    public void SetMoveSpeedModifier(float modifier) => _moveSpeedModifier = modifier;

    public void ResetMoveSpeed() => _moveSpeed = _defaultMoveSpeed;
    public void ResetMoveSpeedModifier() => _moveSpeedModifier = 1f;

    public void UpdateRotation()
    {
        Vector3 finalRotation = _playerCamera.transform.rotation.eulerAngles;
        finalRotation.x = 0;
        finalRotation.z = 0;

        transform.eulerAngles = finalRotation;
    }

}
