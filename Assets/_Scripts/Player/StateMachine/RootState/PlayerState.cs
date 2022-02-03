using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState: AbstractState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    protected Animator animator;
    protected AnimationHolder animationHolder;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, Animator animator, AnimationHolder animationHolder) //: base(player, stateMachine, playerData)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animator = animator;
        this.animationHolder = animationHolder;
        
    }

    protected float moveInputAxis;
    protected bool isJumping;
    protected bool interacting;

    protected string jumpPressedTimer = "JumpPressedTimer";
    protected float jumpTimerTick = 0.37f;
    protected bool onJumpPressedTolerance = false;

    protected float horizontalVelocity;
    protected float processedHorizontalVelocity;
    protected float verticalVelocity;
    protected float groundedVerticalVelocity = -1f;

    protected float smoothInputVelocity; //ref for Mathf.SmoothDamp

    protected bool isTurning = false;
    protected float turnSpeed;

    private bool faceRight = true;
    private const float right = 0;
    private const float left = 180;
    private const float turnSpeedMultiplier = 10f;

    protected int currentDamage;

    public override void Enter()
    {
        //Debug.Log("Now in: " + stateMachine.CurrentState);

        Player.OnTakeDamage += PlayerTakeDamage;

        startTime = Time.time;

        verticalVelocity = player.PlayerVelocity.y;
        horizontalVelocity = player.RawVelocity.x;

    }

    public override void Exit()
    {
        Player.OnTakeDamage -= PlayerTakeDamage;
    }

    public override void LogicUpdate()
    {
        moveInputAxis = player.InputHandler.MoveAxis;
        isJumping = player.InputHandler.IsJumping;
        interacting = player.InputHandler.Interacting;

        if (isJumping)
        {
            if (!Timer.TimerRunning(jumpPressedTimer))
            {
                onJumpPressedTolerance = true;
                Timer.Create(JumpPressedTimeout, jumpTimerTick, jumpPressedTimer);
            }
            //player.InputHandler.JumpStop();
        }

        if (isTurning)
        {
            Turning();
        }
    }

    public override void PhysicsUpdate()
    {

    }

    protected void JumpPressedTimeout()
    {
        if (onJumpPressedTolerance)
            onJumpPressedTolerance = false;
    }

    protected void SpeedChange(float targetSpeed, float momentum)
    {
        horizontalVelocity = Mathf.SmoothDamp(horizontalVelocity, targetSpeed, ref smoothInputVelocity, momentum / 100); //, Time.deltaTime);
    }

    protected void SetPlayerHorizontalVelocity(float horizontalVelocity, float speedModifier)
    {
        processedHorizontalVelocity = horizontalVelocity * speedModifier * Time.deltaTime;
        player.SetHorizontalVelocity(horizontalVelocity, speedModifier);
    }

    protected bool FaceCheck(float moveAxis)
    {
        /*
        if(moveAxis > 0 && !faceRight)
        {
            currentAngle = left;
            targetAngle = right;
            return true;
        }
        else if(moveAxis < 0 && faceRight)
        {
            currentAngle = right;
            targetAngle = left;
            return true;
        }
        */
        return false;
        
    }
    protected void SetTurning(float turnValue)
    {        
        //tempAngle = currentAngle;
        turnSpeed = turnValue;
        isTurning = true;        
    }

    protected void Turning()
    {
        /*
        tempAngle = Mathf.MoveTowardsAngle(tempAngle, targetAngle, (turnSpeed*turnSpeedMultiplier) * Time.deltaTime);
        //player.SetPlayerAngle(tempAngle);

        if (currentAngle == 360)
            currentAngle = 0;

        if (tempAngle == targetAngle)
        {
            if(tempAngle == left)
            {
                faceRight = false;
                isTurning = false;
            }
            else if(tempAngle == right)
            {
                faceRight = true;
                isTurning = false;
            }            
        }
        */
    }

    protected void ForceTurning() //call this to face targetAngle right away (like when jumping)
    {
        /*
        //Debug.Log("forceTurning() called");
        if (moveInputAxis > 0 && currentAngle != right)
        {            
            //targetAngle = right;
            faceRight = true;
            //currentAngle = targetAngle;
            //player.SetPlayerAngle(targetAngle);
        }
        else if (moveInputAxis < 0 && currentAngle != left)
        {
            //targetAngle = left;
            faceRight = false;
            //currentAngle = targetAngle;
            //player.SetPlayerAngle(targetAngle);
        }
        */
    }

    protected void PlayerTakeDamage(int damage)
    {
        //take damage state handler

        currentDamage = damage;
        //stateMachine.ChangeState(player.takeDamageState);
    }
}
