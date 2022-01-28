using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : GroundState
{
    public PlayerIdle(Player player, PlayerStateMachine stateMachine, PlayerData playerData, Animator animator, AnimationHolder animationHolder) : base(player, stateMachine, playerData, animator, animationHolder)
    {

    }

    public override void Enter()
    {
        base.Enter();

        //animator.SetFloat(animationHolder.runningFloat, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(interacting)
        {
            //stateMachine.ChangeState(player.DisabledState);
        }
        
        if (Mathf.Abs(moveInputAxis) > 0.01f)
        {
            //stateMachine.ChangeState(player.MoveState);
        }

        if(!player.IsGrounded)
        {
            //stateMachine.ChangeState(player.FallState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
