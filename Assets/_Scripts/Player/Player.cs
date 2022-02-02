using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Player : MonoBehaviour
{
    public string PlayerName { get { return playerName; } private set { } }
    public Interactable CurrentInteractible { get; private set; }

    #region State Properties
    //public PlayerStateMachine StateMachine { get; private set; }
    //public PlayerIdle IdleState { get; private set; }
    #endregion

    #region Movement Properties
    public Vector2 PlayerVelocity { get { return _playerVelocity; } private set { } }
    public Vector2 RawVelocity { get { return _rawVelocity; } private set { } }
    public bool IsGrounded { get { return GroundCheck(); } private set { } }

    public PlayerInputHandler InputHandler { get { return inputHandler; } private set { } }

    #endregion

    [SerializeField] private string playerName;

    [SerializeField] private PlayerMoveState playerState = PlayerMoveState.idle;

    ///States:
    ///idle
    ///move
    ///interact
    ///jump
    ///land
    ///fall
    ///disabled

    private PlayerInputHandler inputHandler;
    private Animator animator;
    private AnimationHolder animationHolder;
    private Rigidbody2D rb;


    #region Private Variables
    [SerializeField] private PlayerData playerData;
    private Vector2 _playerVelocity = Vector2.zero;
    private Vector2 _rawVelocity = Vector2.zero;
    private Vector2 _movementVelocity = Vector2.zero;

    [SerializeField] private float speed = 10f;
    private float jumpForce = 50;

    #endregion

    #region Player Events
    public static event Action<int> OnTakeDamage;

    //public static event Action<BlockReference> OnCallingDialogue;

    public event Action<Player> OnPlayerInteract;
    #endregion

    //Temporary debug
    public BlockReference tempBlockRef;


    private void Awake()
    {
        //InitializeStateMachine();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void OnEnable()
    {
        MovementManager.OnBlockEnd += EnablingPlayerControl;
        MovementManager.OnBlockStart += DisablingPlayerControl;
    }

    private void OnDisable()
    {
        MovementManager.OnBlockEnd -= EnablingPlayerControl;
        MovementManager.OnBlockStart -= DisablingPlayerControl;
    }


    private void InitializeStateMachine()
    {
        //StateMachine = new PlayerStateMachine();
        //IdleState = new PlayerIdle(this, StateMachine, playerData, animator, animationHolder);
    }

    private void EnablingPlayerControl(string blockName)
    {
        playerState = PlayerMoveState.idle;
        inputHandler.InputActionSwitch(true);
    }

    private void DisablingPlayerControl(string blockName)
    {
        playerState = PlayerMoveState.interacting;
        inputHandler.InputActionSwitch(false);
    }

    void Update()
    {
        _playerVelocity.x = inputHandler.MoveAxis;

        switch (playerState)
        {
            case PlayerMoveState.idle:                

                if(Mathf.Abs(_playerVelocity.x) > 0.01f)
                {
                    playerState = PlayerMoveState.move;
                }
                if (inputHandler.Interacting)
                {
                    //OnCallingDialogue?.Invoke(tempBlockRef);
                    OnPlayerInteract?.Invoke(this);
                    inputHandler.StopInteract();
                    playerState = PlayerMoveState.interacting;
                }              

                break;

            case PlayerMoveState.move:                

                if (Mathf.Abs(_playerVelocity.x) <= 0.01f)
                {
                    playerState = PlayerMoveState.idle;
                }

                break;

            case PlayerMoveState.onAir:

                break;

            case PlayerMoveState.interacting:

                //playerState = PlayerMoveState.idle; //shortcut to direct idle
                break;  
        }        
    }

    private void FixedUpdate()
    {
        if(playerState != PlayerMoveState.interacting || playerState != PlayerMoveState.onAir)
        {
            if(inputHandler.IsJumping)
            {
                Debug.Log("Jump pressed!");
                //playerState = PlayerMoveState.onAir;
                Jump();
            }
        }


        rb.velocity = new Vector2 (_playerVelocity.x * speed * Time.deltaTime, rb.velocity.y);

    }

    private void Jump()
    {
        rb.velocity = new Vector2 (rb.velocity.x, _playerVelocity.y * jumpForce * Time.deltaTime);
    }

    public bool GroundCheck()
    {
        return true;
    }

    public void SetHorizontalVelocity(float horizontalVelocity, float speedModifier)
    {
        _rawVelocity.x = horizontalVelocity;
        _playerVelocity.x = horizontalVelocity * speedModifier;
    }

    public void SetVerticalVelocity(float verticalVelocity)
    {
        _rawVelocity.y = verticalVelocity;
    }

    public void SetInteractible(Interactable currentInteractible)
    {
        CurrentInteractible = currentInteractible;
    }

}

public enum PlayerMoveState
{
    idle,
    move,
    onAir,
    interacting
}
