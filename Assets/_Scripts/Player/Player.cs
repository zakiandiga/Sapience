using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string PlayerName => playerData.characterName;
    public Interactable CurrentInteractable { get; private set; }

    private PlayerRootState rootState = PlayerRootState.OnGround;
    private PlayerMoveState moveState = PlayerMoveState.ready;

    private PlayerInputHandler inputHandler;
    private CharacterAnimation playerAnimation;
    private BoxCollider2D playerCollider;
    private Rigidbody2D rb;
    private float groundCheckBounds = 0.3f;
    [SerializeField] private LayerMask groundLayer;
    private RaycastHit2D groundCheckHit;

    #region Movement Properties & Variables
    [SerializeField] private PlayerData playerData;

    public float VerticalVelocity => rb.velocity.y;
    public float HorizontalVelocity => rb.velocity.x;
    private float accelerationMomentum => playerData.accelerationMomentum;
    private float deccelerationMomentum => playerData.deccelerationMomentum;
    private float turningMomentum => playerData.turningMomentum;
    private float turningSpeedTreshold => playerData.turningSpeedTreshold;
    private float maxSpeed => playerData.maxSpeed;
    [SerializeField] private float jumpForce => playerData.jumpForce;
    [SerializeField] private float gravityMods => playerData.fallForce;
    [SerializeField] private float jumpPressedLimit => playerData.jumpPressLimit;

    private Vector2 _playerVelocity = Vector2.zero;
    private float tempAxis = 0f;
    private float smoothInputVelocity = 1f;
    private float currentSpeed = 80f;
    private bool jumpOngoing = false;
    private bool moveOngoing = false;
    
    private string jumpPressedLimitTimer = "JumpPressedTimer";
    #endregion

    #region Player Events
    public event Action<Player> OnPlayerInteract;
    public static event Action<Transform> OnSetPlayerPosition;
    public static event Action<Transform> OnPlayerDisabled;
    #endregion

    private void Start()
    {
        Debug.Log("Start");
        playerCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponentInChildren<CharacterAnimation>();

    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
        MovementManager.OnBlockEnd += EnablingPlayerControl;
        MovementManager.OnBlockStart += DisablingPlayerControl;
        MovementManager.OnSetPlayerSpawn += SetPlayerPosition;

        inputHandler = GetComponent<PlayerInputHandler>();
        inputHandler.OnJump += JumpTrigger;

    }

    private void OnDisable()
    {
        Debug.Log("Player Disabled");
        MovementManager.OnBlockEnd -= EnablingPlayerControl;
        MovementManager.OnBlockStart -= DisablingPlayerControl;
        MovementManager.OnSetPlayerSpawn -= SetPlayerPosition;
        inputHandler.OnJump -= JumpTrigger;
        OnPlayerDisabled?.Invoke(this.transform);

    }

    private void EnablingPlayerControl(string blockName)
    {
        rootState = PlayerRootState.OnGround;
        CurrentInteractable = null;
        inputHandler.InputActionSwitch(true);
    }

    private void DisablingPlayerControl(string blockName)
    {
        rootState = PlayerRootState.interacting;
        inputHandler.InputActionSwitch(false);
    }

    void Update()
    {
        

        switch (rootState)
        {
            case PlayerRootState.OnGround:                
                if(VerticalVelocity < -0.05f)
                {
                    rootState = PlayerRootState.onAir;
                }

                if(Mathf.Abs(inputHandler.MoveAxis) > 0.01f)
                {
                    if(!moveOngoing)
                        moveOngoing = true;
                }

                if (inputHandler.Interacting)
                {
                    OnPlayerInteract?.Invoke(this);
                }

                break;

            case PlayerRootState.move:
                /*
                if(Mathf.Abs(_playerVelocity.x) >= 0.01f)
                    Debug.Log(_playerVelocity.x);

                if (VerticalVelocity < -0.05f)
                {
                    rootState = PlayerRootState.onAir;
                }

                if(Mathf.Abs(inputHandler.MoveAxis) >= 0.01f)
                {
                    if(ChangeDirection(inputHandler.MoveAxis))
                    {

                    }

                    currentSpeed = Mathf.SmoothDamp(currentSpeed, maxSpeed, ref smoothInputVelocity, accelerationMomentum / 100);
                }
                
                else if (Mathf.Abs(inputHandler.MoveAxis) <= 0.01f)
                {
                    //deccelerating
                    currentSpeed = Mathf.SmoothDamp(currentSpeed, 0, ref smoothInputVelocity, accelerationMomentum / 100);
                    

                    if(Mathf.Abs(currentSpeed) <= 0.1f)
                        rootState = PlayerRootState.OnGround;
                }
                */
                break;

            case PlayerRootState.onAir:
                if(!inputHandler.IsJumpPressed)
                {
                    jumpOngoing = false;
                    if (Timer.TimerRunning(jumpPressedLimitTimer))
                        Timer.ForceStopTimer(jumpPressedLimitTimer);
                }


                break;

            case PlayerRootState.interacting:

                //playerState = PlayerMoveState.idle; //shortcut to direct idle
                break;  
        }        

        if(moveOngoing)
        {
            GroundMove();
        }
    }

    private void FixedUpdate()
    {
        if(rootState == PlayerRootState.onAir)
        {
            if(jumpOngoing)
                Jump();
            else if (!jumpOngoing)
            {
                Fall();

                if (GroundCheck())
                {
                    Debug.Log("Grounded");
                    jumpOngoing = false;
                    rootState = PlayerRootState.OnGround; 
                }
            }
        }

        HorizontalMovement();
    }

    private void GroundMove()
    {
        switch(moveState)
        {
            case PlayerMoveState.ready:

                Debug.Log("Enter Ready moveState");
                tempAxis = inputHandler.MoveAxis;
                Debug.Log("Exit to acceleration");
                moveState = PlayerMoveState.acceleration;
                break;

            case PlayerMoveState.acceleration:
                _playerVelocity.x = tempAxis;
                currentSpeed = Mathf.SmoothDamp(currentSpeed, maxSpeed, ref smoothInputVelocity, accelerationMomentum / 100);

                if(currentSpeed >= maxSpeed * (90f/100f))
                {
                    Debug.Log("Exit to TopSpeed");
                    moveState = PlayerMoveState.topSpeed;
                }

                if(Mathf.Abs(inputHandler.MoveAxis) < 0.1f)
                {
                    Debug.Log("Exit to decceleration");
                    moveState = PlayerMoveState.decceleration;
                }
                break;

            case PlayerMoveState.topSpeed:
                if(Mathf.Abs(inputHandler.MoveAxis) < 0.1f)
                {
                    Debug.Log("Exit from topSpeed to decceleration");
                    moveState = PlayerMoveState.decceleration;
                }
                break;

            case PlayerMoveState.decceleration:
                currentSpeed = Mathf.SmoothDamp(currentSpeed, 0, ref smoothInputVelocity, deccelerationMomentum / 100);

                //If player press directional input
                if(Mathf.Abs(inputHandler.MoveAxis) > 0.01f)
                {
                    //on the opposite direction
                    if(ChangeDirection(inputHandler.MoveAxis))
                    {
                        if(Mathf.Abs(_playerVelocity.x) > 0.01f)
                        {
                            tempAxis *= -1;
                            Debug.Log("Exit to turning");
                            moveState = PlayerMoveState.turning;
                        }
                    }
                    //on the same direction
                    else if(!ChangeDirection(inputHandler.MoveAxis))
                    {
                        Debug.Log("exit to accel");
                        moveState = PlayerMoveState.acceleration;
                    }
                }

                //If no directional input and current speed is almost 0
                else if(Mathf.Abs(inputHandler.MoveAxis) < 0.01f && Mathf.Abs(currentSpeed) < 0.1f)
                {
                    moveState = PlayerMoveState.stop;
                }
                break;

            case PlayerMoveState.turning:

                currentSpeed = Mathf.SmoothDamp(currentSpeed, 0, ref smoothInputVelocity, turningMomentum / 100);
                if (Mathf.Abs(currentSpeed) < turningSpeedTreshold)
                {
                    _playerVelocity.x = tempAxis;
                    moveState = PlayerMoveState.acceleration;
                }
                break;

            case PlayerMoveState.stop:
                tempAxis = 0;
                _playerVelocity.x = tempAxis;
                moveState = PlayerMoveState.ready;
                moveOngoing = false;
                break;
        }
    }

    private void JumpTrigger(PlayerInputHandler inputHandler)
    {
        if(GroundCheck() && (rootState == PlayerRootState.OnGround || rootState == PlayerRootState.move))
        {
            jumpOngoing = true;

            if (!Timer.TimerRunning(jumpPressedLimitTimer))
                Timer.Create(ForceFalling, jumpPressedLimit, jumpPressedLimitTimer);
            
            rootState = PlayerRootState.onAir;
        }
    }

    private void Jump() => rb.velocity = Vector2.up * jumpForce * Time.deltaTime;

    private void Fall() => rb.velocity += Vector2.up * (gravityMods * -1) * Time.deltaTime;

    private void HorizontalMovement() => rb.velocity = new Vector2(_playerVelocity.x * currentSpeed * Time.deltaTime, rb.velocity.y);

    public bool GroundCheck()
    {
        groundCheckHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0, Vector2.down, groundCheckBounds, groundLayer);
        
        Color rayColor = groundCheckHit.collider != null ? Color.green : Color.red;
        Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.extents.y + groundCheckBounds), rayColor);
        
        return groundCheckHit.collider != null;
    } 

    private bool ChangeDirection(float inputAxis)
    {
        if ((inputAxis > 0 && HorizontalVelocity > 0) || (inputAxis < 0 && HorizontalVelocity < 0))
            return false;
        else
            return true;        
    }

    public void SetInteractible(Interactable currentInteractible) => CurrentInteractable = currentInteractible;

    private void SetPlayerPosition(Transform targetPosition)
    {
        transform.position = targetPosition.position;
        OnSetPlayerPosition?.Invoke(this.transform);
    }

    private void ForceFalling()
    {
        if (jumpOngoing)
            jumpOngoing = false;
    }
}

public enum PlayerRootState
{
    OnGround,
    move,
    onAir,
    interacting
}

public enum PlayerMoveState
{
    ready,
    acceleration,
    topSpeed,
    decceleration,
    turning,
    stop
}
