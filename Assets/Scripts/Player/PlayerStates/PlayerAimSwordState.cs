using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : BasePlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skillManager.swordSkill.ShowDots(true);
    }

    public override void Exit()
    {
        base.Exit();

        // Don't let player slide after aim
        player.StartCoroutine("BusyFor", 0.2f);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        // Move back to idle when right mouse button is released, throwing the sword is called by ThrowSword Animation Trigger
        if(Input.GetKeyUp(KeyCode.Mouse1)) 
        {
            stateMachine.ChangeState(player.idleState);
        }

        FlipPlayerOnMouseAim();
    }

    private void FlipPlayerOnMouseAim()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // If mouse is on left side of player during aim and player is facing right then flip 
        if(mousePosition.x < player.transform.position.x && player.facingDirection == 1) player.Flip();

        // if mouse is on right side of the player during aim and player is facing left then flip
        if(mousePosition.x > player.transform.position.x && player.facingDirection == -1) player.Flip();
    }
}
