using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private InputActionAsset _inputActions;

    

    private void Awake()
    {
        _inputActions = InputSystem.actions;
    }

    private void Start()
    {
        
    }

}
