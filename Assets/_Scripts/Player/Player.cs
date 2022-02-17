using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string PlayerName => playerData.characterName;
    public Interactable CurrentInteractible { get; private set; }

    private PlayerMoveState playerState = PlayerMoveState.idle;

    private PlayerInputHandler inputHandler;
    private CharacterAnimation playerAnimation;
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
    public event Action<Player> OnPlayerInteract;
    public static event Action<Transform> OnSetPlayerPosition;
    public static event Action<Transform> OnPlayerDisabled;
    #endregion

    private void Awake()
    {
        //InitializeStateMachine();
    }

    private void Start()
    {        
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<PlayerInputHandler>();
        playerAnimation = GetComponentInChildren<CharacterAnimation>();
    }

    private void OnEnable()
    {        
        MovementManager.OnBlockEnd += EnablingPlayerControl;
        MovementManager.OnBlockStart += DisablingPlayerControl;
        MovementManager.OnSetPlayerSpawn += SetPlayerPosition;
    }

    private void OnDisable()
    {
        Debug.Log("Player Disabled");
        MovementManager.OnBlockEnd -= EnablingPlayerControl;
        MovementManager.OnBlockStart -= DisablingPlayerControl;
        MovementManager.OnSetPlayerSpawn -= SetPlayerPosition;
        OnPlayerDisabled?.Invoke(this.transform);

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
                    Debug.Log("Player INTERACTING");
                    //OnCallingDialogue?.Invoke(tempBlockRef);
                    OnPlayerInteract?.Invoke(this);
                    //inputHandler.StopInteract();
                    //playerState = PlayerMoveState.interacting;
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

    private void SetPlayerPosition(Transform targetPosition)
    {
        transform.position = targetPosition.position;
        OnSetPlayerPosition?.Invoke(this.transform);
    }

}

public enum PlayerMoveState
{
    idle,
    move,
    onAir,
    interacting
}
