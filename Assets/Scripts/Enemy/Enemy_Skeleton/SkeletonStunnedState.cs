using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton enemy;
    public SkeletonStunnedState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Enemy_Skeleton _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stunnedTime;
        rb.velocity = new Vector2(enemy.stunnedDirection.x * -enemy.facingDirection, enemy.stunnedDirection.y);

        enemy.entityFX.InvokeRepeating("StunnedRedBlink", 0f, 0.1f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer < 0)
        {
            enemy.entityFX.Invoke("CancelStunnedRedBlink", 0f);
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
