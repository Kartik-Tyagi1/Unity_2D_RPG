using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : BasePlayerState
{
    private Transform sword;

    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;

        // If mouse is on left side of player during aim and player is facing right then flip 
        if(sword.position.x < player.transform.position.x && player.facingDirection == 1) player.Flip();

        // if mouse is on right side of the player during aim and player is facing left then flip
        if(sword.position.x > player.transform.position.x && player.facingDirection == -1) player.Flip();

        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDirection, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        // Don't let player move, to cancel knockback of sword
        player.StartCoroutine("BusyFor", 0.1f);
    }

    public override void Update()
    {
        base.Update(); 

        // On CatchSwordAnim AnimationTriggerCalled
        if(triggerCalled) stateMachine.ChangeState(player.idleState);
    }
}
