using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : BasePlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;

        player.skillManager.cloneSkill.CreateClone(player.transform);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0f, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if(!player.IsWallDetected() && player.IsWallDetected()) stateMachine.ChangeState(player.wallslideState);

        player.SetVelocity(player.dashDirection * player.dashSpeed, 0);
        if(stateTimer < 0f) stateMachine.ChangeState(player.idleState);
    }
}
