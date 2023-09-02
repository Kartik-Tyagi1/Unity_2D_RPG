using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallslideState : BasePlayerState
{
    public PlayerWallslideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        // Go to wall jump is player jumps during wall slide, and we want to exit so other code does not interupt
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.walljumpState);
            return;
        }

        // Slide on the wall at full speed if pressing down, else slide at 70% speed
        if (yInput < 0) rb.velocity = new Vector2(0, rb.velocity.y);
        else rb.velocity = new Vector2(0, rb.velocity.y * 0.7f);

        // Go back to idle if the player moves off the wall on lands on the ground
        if ((xInput != 0 && player.facingDirection != xInput) || player.IsOnGround()) stateMachine.ChangeState(player.idleState);
        
    }
}
