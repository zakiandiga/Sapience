using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string PlayerName => playerData.characterName;
    public Interactable CurrentInteractable { get; private set; }

    private PlayerRootState rootState = PlayerRootState.onGround;
    private PlayerMoveState moveState = PlayerMoveState.ready;

    #region Player's Components
    private PlayerInputHandler inputHandler;
    //private CharacterAnimation playerAnimation;
    private BoxCollider2D playerCollider;
    private Rigidbody2D rb;
    [SerializeField] private Transform x3Sprite, x4Sprite;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private playerAnimationEventAudio animationEventAudio;
    #endregion

    #region GroundChecks
    private float groundCheckBounds = 0.3f;
    [SerializeField] private LayerMask groundLayer;
    private RaycastHit2D groundCheckHit;
    #endregion

    #region Movement Properties & Variables
    [SerializeField] private PlayerData playerData;

    public float VerticalVelocity => rb.velocity.y;
    public float HorizontalVelocity => rb.velocity.x;
    private float accelerationMomentum => playerData.accelerationMomentum;
    private float deccelerationMomentum => playerData.deccelerationMomentum;
    private float turningMomentum => playerData.turningMomentum;
    private float turningSpeedTreshold => playerData.turningSpeedTreshold;
    private float maxSpeed => playerData.maxSpeed;
    private float jumpForce => playerData.jumpForce;
    private float gravityMods => playerData.fallForce;
    private float jumpPressedLimit => playerData.jumpPressLimit;

    private Vector2 _playerVelocity = Vector2.zero;
    private float tempAxis = 0f;
    private float smoothInputVelocity = 1f;
    private float currentSpeed = 80f;
    private float minimumSpeedToStop = 15f;
    private bool jumpOngoing = false;
    private bool moveOngoing = false;
    
    private string jumpPressedLimitTimer = "JumpPressedTimer";

    private bool jumpTriggered = false;
    private float jumpTriggerTreshold = 0.3f;
    private string jumpTriggerTresholdTimer = "JumpTriggerTimer";
    #endregion

    #region Player Events
    public event Action<Player> OnPlayerInteract;
    public static event Action<Player> OnPlayerEnabled;
    public static event Action<Transform> OnSetPlayerPosition;
    public static event Action<Transform> OnPlayerDisabled;
    #endregion

    private void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        //playerAnimation = GetComponentInChildren<CharacterAnimation>();

    }

    private void OnEnable()
    {
        MovementManager.OnBlockEnd += EnablingPlayerControl;
        MovementManager.OnBlockStart += DisablingPlayerControl;
        MovementManager.OnSetPlayerSpawn += SetPlayerPosition;

        inputHandler = GetComponent<PlayerInputHandler>();
        inputHandler.OnJump += JumpTrigger;

        OnPlayerEnabled?.Invoke(this);

    }

    private void OnDisable()
    {
        MovementManager.OnBlockEnd -= EnablingPlayerControl;
        MovementManager.OnBlockStart -= DisablingPlayerControl;
        MovementManager.OnSetPlayerSpawn -= SetPlayerPosition;
        inputHandler.OnJump -= JumpTrigger;
        OnPlayerDisabled?.Invoke(this.transform);

    }

    private void EnablingPlayerControl(string blockName)
    {
        if (GroundCheck())
        {
            anim.SetBool("OnGround", true);
            rootState = PlayerRootState.onGround;
        }
        else
            rootState = PlayerRootState.onAir;

        CurrentInteractable = null;
        inputHandler.InputActionSwitch(true);
    }

    private void DisablingPlayerControl(string blockName)
    {
        if (anim != null)
        {
            anim.SetBool("IsWalking", false);
            anim.SetTrigger("Land");
        }

        //rootState = PlayerRootState.interacting;
        /*
        if(moveState != PlayerMoveState.ready)
        {
            tempAxis = 0;
            currentSpeed = 0;
            _playerVelocity.x = tempAxis;
            moveState = PlayerMoveState.ready;
            moveOngoing = false;
        }
        */
        inputHandler.InputActionSwitch(false);
    }

    void Update()
    {

        switch (rootState)
        {
            case PlayerRootState.onGround:

                if(VerticalVelocity < -0.5f)
                {
                    rootState = PlayerRootState.onAir;
                }

                if (inputHandler.Interacting)
                {
                    OnPlayerInteract?.Invoke(this);
                    //rootState = PlayerRootState.interacting;
                }

                break;            

            case PlayerRootState.onAir:
                if (!inputHandler.IsJumpPressed)
                {
                    jumpOngoing = false;
                    if (Timer.TimerRunning(jumpPressedLimitTimer))
                        Timer.ForceStopTimer(jumpPressedLimitTimer);
                }

                if (GroundCheck() && !jumpOngoing)
                    rootState = PlayerRootState.landing;
                
                
                break;

            case PlayerRootState.landing:
                animationEventAudio.PlayAudio(3);
                if (jumpTriggered)
                {
                    jumpTriggered = false;
                    //Debug.Log("REJUMP");
                    NormalizeVerticalMovement();
                    StartJumping();
                }

                if (anim != null)
                {
                    anim.SetTrigger("Land");
                }

                if (anim != null)
                    anim.SetBool("OnGround", true);
                rootState = PlayerRootState.onGround;


                break;

            case PlayerRootState.interacting:
                

                /*
                if (moveState != PlayerMoveState.ready)
                {

                    tempAxis = 0;
                    currentSpeed = 0;
                    _playerVelocity.x = tempAxis;
                    moveState = PlayerMoveState.ready;
                    moveOngoing = false;
                }
                */
                //playerState = PlayerMoveState.idle; //shortcut to direct idle
                break;  
        }

        if (Mathf.Abs(inputHandler.MoveAxis) > 0.01f)
        {
            if (!moveOngoing)
                moveOngoing = true;
        }

        if (moveOngoing)
        {
            GroundMove();
        }
    }

    private void FixedUpdate()
    {

        if (rootState == PlayerRootState.onAir)
        {
            if (jumpOngoing)
            {
                Jump();
            }
            else
            {
                Fall();
            }

        }

        HorizontalMovement();
    }

    #region Movement Logic
    private void GroundMove()
    {
        if(spriteRenderer != null)
        {
            if (inputHandler.MoveAxis > 0 && spriteRenderer.flipX)
                spriteRenderer.flipX = false;
            if (inputHandler.MoveAxis < 0 && !spriteRenderer.flipX)
                spriteRenderer.flipX = true;
        }

        _playerVelocity.x = inputHandler.MoveAxis;


        currentSpeed = maxSpeed;

        if (anim != null)
        {
            if (Mathf.Abs(HorizontalVelocity) > 0.01f)
                anim.SetBool("IsWalking", true);
            else
                anim.SetBool("IsWalking", false);        

        }


        #region Bugged
        /*
        switch(moveState)
        {
            case PlayerMoveState.ready:

                //Debug.Log("Enter Ready moveState");
                tempAxis = 0;
                tempAxis = inputHandler.MoveAxis;
                //Debug.Log("Exit to acceleration");
                _playerVelocity.x = tempAxis;
                if(Mathf.Abs(_playerVelocity.x) > 0.01f)
                    moveState = PlayerMoveState.acceleration;
                break;

            case PlayerMoveState.acceleration:
                _playerVelocity.x = tempAxis;
                currentSpeed = Mathf.SmoothDamp(currentSpeed, maxSpeed, ref smoothInputVelocity, accelerationMomentum / 100);

                if(currentSpeed >= maxSpeed * (90f/100f))
                {
                    //Debug.Log("Exit to TopSpeed");
                    moveState = PlayerMoveState.topSpeed;
                }

                if(Mathf.Abs(inputHandler.MoveAxis) < 0.1f)
                {
                    //Debug.Log("Exit to decceleration");
                    moveState = PlayerMoveState.decceleration;
                }
                break;

            case PlayerMoveState.topSpeed:
                if(Mathf.Abs(inputHandler.MoveAxis) < 0.1f)
                {
                    //Debug.Log("Exit from topSpeed to decceleration");
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
                else if(Mathf.Abs(inputHandler.MoveAxis) < 0.01f && Mathf.Abs(currentSpeed) < minimumSpeedToStop)
                {
                    moveState = PlayerMoveState.stop;
                }
                break;

            case PlayerMoveState.turning:

                currentSpeed = Mathf.SmoothDamp(currentSpeed, 0, ref smoothInputVelocity, turningMomentum / 100);
                if (ChangeDirection(inputHandler.MoveAxis))

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
        */
        #endregion
    }

    private void ForceFalling()
    {
        if (jumpOngoing)
            jumpOngoing = false;
    }

    private void JumpTriggerSwitch()
    {
        if (jumpTriggered)
            jumpTriggered = false;
    }

    private void StartJumping()
    {
        jumpOngoing = true;

        //Stop jump trigger treshold timer if it's running
        if (Timer.TimerRunning(jumpTriggerTresholdTimer))
        {
            Timer.ForceStopTimer(jumpTriggerTresholdTimer);
            JumpTriggerSwitch();
        }

        //start jump hold limit timer if it's not running
        if (!Timer.TimerRunning(jumpPressedLimitTimer))
            Timer.Create(ForceFalling, jumpPressedLimit, jumpPressedLimitTimer);

        if (anim != null)
        {
            anim.SetBool("OnGround", false);
            anim.Play("Jump");            
        }

        rootState = PlayerRootState.onAir;
    }

    private void JumpTrigger(PlayerInputHandler inputHandler)
    {
        if (GroundCheck() && rootState == PlayerRootState.onGround)
        {
            StartJumping();
        }

        else if(!GroundCheck())
        {
            if (!Timer.TimerRunning(jumpTriggerTresholdTimer))
            {
                Timer.Create(JumpTriggerSwitch, jumpTriggerTreshold, jumpTriggerTresholdTimer);
                jumpTriggered = true;
            }
        }
    }

    private void Jump() => rb.velocity = Vector2.up * jumpForce * Time.deltaTime;

    private void Fall()
    {
        rb.velocity += Vector2.up * (gravityMods * -1) * Time.deltaTime;
    }

    private void NormalizeVerticalMovement() => rb.velocity = Vector2.up * 0;

    private void HorizontalMovement()
    {
        rb.velocity = new Vector2(_playerVelocity.x * currentSpeed * Time.deltaTime, rb.velocity.y);
    } 


    public bool GroundCheck()
    {
        groundCheckHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0, Vector2.down, groundCheckBounds, groundLayer);
        
        //Color rayColor = groundCheckHit.collider != null ? Color.green : Color.red;
        //Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.extents.y + groundCheckBounds), rayColor);
        
        return groundCheckHit.collider != null;
    } 

    private bool ChangeDirection(float inputAxis)
    {
        if ((inputAxis > 0 && HorizontalVelocity > 0) || (inputAxis < 0 && HorizontalVelocity < 0))
            return false;
        else
            return true;        
    }
    #endregion

    public void SetInteractible(Interactable currentInteractible) => CurrentInteractable = currentInteractible;

    private void SetPlayerPosition(Transform targetPosition, int characterCode)
    {
        transform.position = targetPosition.position;

        if(characterCode == 1)
        {
            if (x4Sprite.gameObject.activeSelf)
                x4Sprite.gameObject.SetActive(false);
            
            if(!x3Sprite.gameObject.activeSelf)
                x3Sprite.gameObject.SetActive(true);
            anim = x3Sprite.GetComponent<Animator>();
            spriteRenderer = x3Sprite.GetComponent<SpriteRenderer>();
            animationEventAudio = x3Sprite.GetComponent<playerAnimationEventAudio>();
        }
        else if(characterCode == 2)
        {
            if (x3Sprite.gameObject.activeSelf)
                x3Sprite.gameObject.SetActive(false);

            if (!x4Sprite.gameObject.activeSelf)
                x4Sprite.gameObject.SetActive(true);
            anim = x4Sprite.GetComponent<Animator>();
            spriteRenderer = x4Sprite.GetComponent<SpriteRenderer>();
            animationEventAudio = x4Sprite.GetComponent<playerAnimationEventAudio>();
        }

        OnSetPlayerPosition?.Invoke(this.transform);
    }

}

public enum PlayerRootState
{
    onGround,
    onAir,
    interacting,
    landing
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
