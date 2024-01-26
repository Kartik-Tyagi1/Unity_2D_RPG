using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Base state for all states that happen on the ground
 *  - IdleState
 *  - AimSwordState
 *  - CounterAttackState
 *  - JumpState
 *  - AttackState
 *  
 * Moves to air state is no ground is detected as failsafe
 */

public class PlayerGroundedState : BasePlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        // Only transition if player has not thrown the sword
        if(Input.GetKeyDown(KeyCode.Mouse1) && !HasSword()) stateMachine.ChangeState(player.aimSwordState);

        if (Input.GetKeyDown(KeyCode.Q) && player.IsGroundDetected()) stateMachine.ChangeState(player.counterAttackState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected()) stateMachine.ChangeState(player.jumpState);

        if(!player.IsGroundDetected()) stateMachine.ChangeState(player.airState);

        if(Input.GetKeyDown(KeyCode.Mouse0)) stateMachine.ChangeState(player.primaryAttackState);
    }

    public bool HasSword()
    {
        // Player has thrown the sword, return it back to player
        if(player.sword != null)
        {
            player.sword.GetComponent<SwordSkillController>().ReturnSwordToPlayer();
            return true;
        }

        // Player has not thrown the sword
        return false;
    }
}
