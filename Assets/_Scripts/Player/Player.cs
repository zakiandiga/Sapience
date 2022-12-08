using System;
using UnityEngine;

public class Player : MonoBehaviour
{
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
    private float jumpOnCliffFallLimit => playerData.jumpPressLimit;

    private Vector2 _playerVelocity = Vector2.zero;
    private float tempAxis = 0f;
    private float smoothInputVelocity = 1f;
    private float currentSpeed = 80f;
    private float minimumSpeedToStop = 15f;
    private bool jumpOngoing = false;
    private bool moveOngoing = false;
    
    private string jumpOnCliffFallTimer = "JumpPressedTimer";

    private bool jumpCommandQueued = false;
    private float jumpTriggerTreshold = 0.3f;
    private string jumpCommandQueueTimer = "JumpTriggerTimer";
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
        CurrentInteractable = null;
        inputHandler.InputActionSwitch(true);

        if (!GroundCheck())
        {
            rootState = PlayerRootState.onAir;
            return;
        }
       
        anim.SetBool("OnGround", true);
        rootState = PlayerRootState.onGround; 
    }

    private void DisablingPlayerControl(string blockName)
    {
        inputHandler.InputActionSwitch(false);

        if (anim == null)
            return;
       
        anim.SetBool("IsWalking", false);
        anim.SetTrigger("Land");    
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
                }

                break;            

            case PlayerRootState.onAir:
                if (!inputHandler.IsJumpPressed)
                {
                    jumpOngoing = false;
                    if (Timer.TimerRunning(jumpOnCliffFallTimer))
                        Timer.ForceStopTimer(jumpOnCliffFallTimer);
                }

                if (GroundCheck() && !jumpOngoing)
                    rootState = PlayerRootState.landing;                
                
                break;

            case PlayerRootState.landing:
                animationEventAudio.PlayAudio(3);
                if (jumpCommandQueued)
                {
                    jumpCommandQueued = false;
                    NormalizeVerticalMovement();
                    StartJumping();
                }

                if (anim != null)
                {
                    anim.SetTrigger("Land");
                    anim.SetBool("OnGround", true);
                }

                rootState = PlayerRootState.onGround;

                break;

            case PlayerRootState.interacting:

                break;  
        }

        if (Mathf.Abs(inputHandler.MoveAxis) > 0.01f)
        {
            moveOngoing = true;
        }

        if (!moveOngoing)
            return;
        
        GroundMove();
    }

    private void FixedUpdate()
    {
        HandleOnAirMovement();

        HorizontalMovement();
    }



    #region Movement Logic
    private void GroundMove()
    {
        _playerVelocity.x = inputHandler.MoveAxis;
        currentSpeed = maxSpeed;

        HandleSpriteFlip();

        HandleWalkingAnimation();  
    }

    private void HandleWalkingAnimation()
    {
        if (anim == null)
            return;

        if (Mathf.Abs(HorizontalVelocity) < 0.01f)
        {
            anim.SetBool("IsWalking", false);
            return;
        }
        
        anim.SetBool("IsWalking", true);
    }

    private void HandleSpriteFlip()
    {
        if (spriteRenderer == null || _playerVelocity.x == 0)
            return;

        spriteRenderer.flipX = _playerVelocity.x > 0 ? false : true;   
    }

    private void HandleOnAirMovement()
    {
        if (rootState != PlayerRootState.onAir)
            return;

        if (!jumpOngoing)
        {
            Fall();
            return;
        }

        Jump();
    }

    private void ForceFalling()
    {
        if (jumpOngoing)
            jumpOngoing = false;
    }

    private void JumpTriggerSwitch()
    {
        if (jumpCommandQueued)
            jumpCommandQueued = false;
    }

    private void StartJumping()
    {
        jumpOngoing = true;
        rootState = PlayerRootState.onAir;
        HandleJumpingAnimation();

        if (Timer.TimerRunning(jumpCommandQueueTimer))
        {
            Timer.ForceStopTimer(jumpCommandQueueTimer);
            JumpTriggerSwitch();
        }

        if (!Timer.TimerRunning(jumpOnCliffFallTimer))
            Timer.Create(ForceFalling, jumpOnCliffFallLimit, jumpOnCliffFallTimer);        
    }

    private void HandleJumpingAnimation()
    {
        if (anim == null)
            return;

        anim.SetBool("OnGround", false);
        anim.Play("Jump");
    }

    private void JumpTrigger(PlayerInputHandler inputHandler)
    {
        if (GroundCheck() && rootState == PlayerRootState.onGround)
        {
            StartJumping();
            return;
        }

        if (Timer.TimerRunning(jumpCommandQueueTimer))
            return;        

        Timer.Create(JumpTriggerSwitch, jumpTriggerTreshold, jumpCommandQueueTimer);
        jumpCommandQueued = true;        
    }

    private void Jump() => rb.velocity = Vector2.up * jumpForce * Time.deltaTime;

    private void Fall() => rb.velocity += Vector2.up * (gravityMods * -1) * Time.deltaTime;
    
    private void NormalizeVerticalMovement() => rb.velocity = Vector2.up * 0;

    private void HorizontalMovement() => rb.velocity = new Vector2(_playerVelocity.x * currentSpeed * Time.deltaTime, rb.velocity.y);

    public bool GroundCheck()
    {
        RaycastHit2D groundCheckHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0, Vector2.down, groundCheckBounds, groundLayer);
        return groundCheckHit.collider != null;
    } 
    #endregion

    public void SetInteractible(Interactable currentInteractible) => CurrentInteractable = currentInteractible;

    private void SetPlayerPosition(Transform targetPosition, int characterCode)
    {
        transform.position = targetPosition.position;
        OnSetPlayerPosition?.Invoke(this.transform);

        if(characterCode == 1)
        {
            SwitchPlayerSprite(x4Sprite, x3Sprite);            
        }
        else if(characterCode == 2)
        {
            SwitchPlayerSprite(x3Sprite, x4Sprite);           
        }
    }

    private void SwitchPlayerSprite(Transform oldSprite, Transform newSprite)
    {
        oldSprite.gameObject.SetActive(false);
        newSprite.gameObject.SetActive(true);
        anim = newSprite.GetComponent<Animator>();
        spriteRenderer = newSprite.GetComponent<SpriteRenderer>();
        animationEventAudio = newSprite.GetComponent<playerAnimationEventAudio>();
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
