using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference run;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference interact;

    public event Action<PlayerInputHandler> OnJump;
    public float MoveAxis => run.action.ReadValue<float>();
    public bool IsJumpPressed => jump.action.ReadValue<float>() > 0 ? true : false;
    public bool Interacting => interact.action.triggered;

    private void OnEnable()
    {        
        InputActionSwitch(true);
    }
    private void OnDisable()
    {
        InputActionSwitch(false);
    }

    public void InputActionSwitch(bool enabling)
    {
        if (enabling)
        {
            run.action.Enable();
            jump.action.Enable();
            interact.action.Enable();

            jump.action.started += Jump;
        }
        else if (!enabling)
        {
            run.action.Disable();
            jump.action.Disable();
            interact.action.Disable();

            jump.action.started -= Jump;
        }
    }   
    
    private void Jump(InputAction.CallbackContext context)
    {
        OnJump?.Invoke(this);
    }
}