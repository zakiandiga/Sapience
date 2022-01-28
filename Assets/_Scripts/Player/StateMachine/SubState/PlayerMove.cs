using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : GroundState
{
    public PlayerMove(Player player, PlayerStateMachine stateMachine, PlayerData playerData, Animator animator, AnimationHolder animationHolder) : base(player, stateMachine, playerData, animator, animationHolder)
    {

    }

    public enum MoveState
    {
        ready,
        acceleration,
        topSpeed,
        decceleration,
        stop
    }

    private MoveState moveState = MoveState.ready;
    private float tempAxis;

    public override void Enter()
    {
        base.Enter();
        moveState = MoveState.ready;

        animator.SetBool(animationHolder.runningBool, true);

        /*
        if (stateMachine.LastState == player.LandState)
        {
            moveState = MoveState.decceleration;
        }
        else
            moveState = MoveState.ready;
        */
    }

    public override void Exit()
    {
        base.Exit();
        //animator.SetBool(animationHolder.runningBool, false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Moving();

        if(!player.IsGrounded)
        {
            //stateMachine.ChangeState(player.FallState);
        }

        if(interacting)
        {
            //stateMachine.ChangeState(player.InteractState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void Moving()
    {
        switch(moveState)
        {
            case MoveState.ready:
                tempAxis = 0;

                ExitFromReady();
                break;

        }
        animator.SetFloat(animationHolder.runningFloat, Mathf.Abs(horizontalVelocity));
        SetPlayerHorizontalVelocity(horizontalVelocity, playerData.groundSpeed);
    }

    #region Movement Micro-State Exits
    private void ExitFromReady()
    {
        if(Mathf.Abs(moveInputAxis) > 0)
        {
            tempAxis = moveInputAxis;
            moveState = MoveState.topSpeed;
        }
    }
    #endregion
}
