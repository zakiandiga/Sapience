using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : PlayerState
{

    public AirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, Animator animator, AnimationHolder animationHolder) : base(player, stateMachine, playerData, animator, animationHolder)
    {

    }

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
        Gravity();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    protected void Gravity()
    {
        //OnAir
        verticalVelocity = verticalVelocity + playerData.gravityValue * Time.deltaTime;
        player.SetVerticalVelocity(verticalVelocity);
    }
}
