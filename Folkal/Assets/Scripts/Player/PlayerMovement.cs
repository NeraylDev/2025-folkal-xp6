using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : PlayerSubsystem
{
    [SerializeField] private UIEvents _uiEvents;
    [Space]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _runningSpeed;
    [SerializeField] private float _throwingSpeed;
    private float _defaultMoveSpeed;
    private float _moveSpeedModifier = 1f;
    private Vector2 _inputDirection;
    private Vector3 _moveDirection;
    private bool _canMove;

    private bool _isRunning;

    private Camera _camera;
    private Rigidbody _rigidBody;

    public bool IsRunning => _isRunning;
    public bool CanMove => _canMove;
    public Vector2 GetInputDirection => _inputDirection;
    public Vector3 GetMoveDirection => _moveDirection;


    #region MonoBehaviour Methods

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _defaultMoveSpeed = _moveSpeed;
        _canMove = true;
    }

    private void Start()
    {
        _camera = Camera.main;
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

    protected override void SetEvents(InputActionAsset actionAsset)
    {
        if (_uiEvents == null)
            return;

        actionAsset.FindAction("Move").performed += (InputAction.CallbackContext context)
            => SetInputDirection(context.ReadValue<Vector2>());
        actionAsset.FindAction("Move").canceled += (InputAction.CallbackContext context)
            => SetInputDirection(context.ReadValue<Vector2>());

        actionAsset.FindAction("Run").started += (InputAction.CallbackContext context)
            => ActivateRunning();
        actionAsset.FindAction("Run").canceled += (InputAction.CallbackContext context)
            => DeactivateRunning();

        _uiEvents.onSpeechStart += () => SetCanMove(false);
        _uiEvents.onSpeechEnd += () => SetCanMove(true);
    }

    public void Move()
    {
        if (!_canMove)
            return;

        _moveDirection = (transform.right * _inputDirection.x)
                         + (transform.forward * _inputDirection.y);

        if (_moveDirection.magnitude > 1)
            _moveDirection = _moveDirection.normalized;

        Vector3 finalVelocity = _moveDirection * _moveSpeedModifier * Time.fixedDeltaTime;
        finalVelocity *= _moveSpeed;

        _rigidBody.linearVelocity = finalVelocity;
    }

    private void SetInputDirection(Vector2 direction)
        => _inputDirection = direction;

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

    public void ActivateRunning()
    {
        if (!_canMove || _inputDirection == Vector2.zero
            || Vector2.Dot(_inputDirection, Vector2.up) <= 0
            || _playerManager.GetPlayerThrowing.IsChargingThrow)
            return;

        _isRunning = true;
    }

    public void DeactivateRunning()
    {
        if (_playerManager.GetPlayerThrowing.IsChargingThrow)
            return;

        _isRunning = false;
    }

    public void SetMoveSpeed(float speed) => _moveSpeed = speed;
    public void SetMoveSpeedModifier(float modifier) => _moveSpeedModifier = modifier;

    public void ResetMoveSpeed() => _moveSpeed = _defaultMoveSpeed;
    public void ResetMoveSpeedModifier() => _moveSpeedModifier = 1f;

    public void UpdateRotation()
    {
        Vector3 finalRotation = _camera.transform.rotation.eulerAngles;
        finalRotation.x = 0;
        finalRotation.z = 0;

        transform.eulerAngles = finalRotation;
    }

}
