using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionState : PlayerState
{
    public ActionState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, Animator animator, AnimationHolder animationHolder) : base(player, stateMachine, playerData, animator, animationHolder)
    {

    }

    protected bool actionFinished;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (actionFinished)
        {
            
            if (player.IsGrounded)// && player.PlayerVelocity.y < 0)
            {
                //stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                //stateMachine.ChangeState(player.FallState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


}
