using System;
using UnityEngine;

public class PlayerDisabled : PlayerState
{
    public PlayerDisabled(Player player, PlayerStateMachine stateMachine, PlayerData playerData, Animator animator, AnimationHolder animationHolder) : base(player, stateMachine, playerData, animator, animationHolder)
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
    }


}
