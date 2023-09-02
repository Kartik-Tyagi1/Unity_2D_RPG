using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : EnemyState
{
    private Enemy_Skeleton enemy;

    public SkeletonMoveState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Enemy_Skeleton _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
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
        
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDirection, enemy.rb.velocity.y);

        if(!enemy.IsGroundDetected() || enemy.IsWallDetected())
        {
            enemy.ZeroVelocity();
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
