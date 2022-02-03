using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference run;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference interact;

    public float MoveAxis => run.action.ReadValue<float>();
    public bool IsJumping => jump.action.triggered;
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
        }
        else if (!enabling)
        {
            run.action.Disable();
            jump.action.Disable();
            interact.action.Disable();            
        }
    }     
}