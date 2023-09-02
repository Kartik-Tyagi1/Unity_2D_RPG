using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : BasePlayerState
{
    private const string COUNTER_ATTACK_SUCCESSFUL = "CounterAttackSuccessful";

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.counterAttackTime; // put player into counter attack animation, for short time to not make it easy
        player.animator.SetBool(COUNTER_ATTACK_SUCCESSFUL, false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (Collider2D enemyHit in enemiesHit)
        {
            if (enemyHit.GetComponent<Enemy>() != null)
            {
                // If any of the enemies in the circle get attacked when the counter time is active, stun them and play the animation
                if(enemyHit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10; // Has to be longer than 10
                    player.animator.SetBool(COUNTER_ATTACK_SUCCESSFUL, true);
                }
            }
        }

        // Get out if we miss the counter time or if there is successful counter
        if(stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
