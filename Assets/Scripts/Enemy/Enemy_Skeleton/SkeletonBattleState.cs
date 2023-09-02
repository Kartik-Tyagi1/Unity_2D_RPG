using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Enemy_Skeleton enemy;
    private int moveDirection;

    public SkeletonBattleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Enemy_Skeleton _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform; // TODO: CHANGE THIS
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected().distance < enemy.attackDistance) 
        {
            enemy.ZeroVelocity();
            return;
        }

        enemy.SetVelocity(enemy.moveSpeed * DirectionToPlayer(), rb.velocity.y);
    }

    private int DirectionToPlayer()
    {
        if (player.position.x > enemy.transform.position.x)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
