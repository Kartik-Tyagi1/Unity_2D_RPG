using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : BasePlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

    public override void Update()
    {
        base.Update();
        if (player.IsGroundDetected()) stateMachine.ChangeState(player.idleState);

        // Player only gets 80% of speed in the air
        if (xInput != 0) player.SetVelocity(xInput * 0.8f * player.moveSpeed, rb.velocity.y);

        if(player.IsWallDetected()) stateMachine.ChangeState(player.wallslideState);
    }
}
