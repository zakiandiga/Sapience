using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference run;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference interact;

    public float MoveAxis
    {
        get
        {
            return run.action.ReadValue<float>();
        }
        private set
        {
            run.action.ReadValue<float>();
        }
    }

    public bool IsJumping { get; private set; }
    public bool Interacting { get; private set; }



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
            jump.action.canceled += Jump;
            interact.action.started += Interact;
            interact.action.canceled += Interact;

        }
        else if (!enabling)
        {
            run.action.Disable();
            jump.action.Disable();
            interact.action.Disable();

            jump.action.started -= Jump;
            jump.action.canceled -= Jump;
            interact.action.started -= Interact;
            interact.action.canceled -= Interact;
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Interacting = true;
        }

    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsJumping = true;
        }
        if (context.canceled)
        {
            IsJumping = false;
        }
    }

    public void JumpStop() => IsJumping = false;

    public void StopInteract() => Interacting = false;

}