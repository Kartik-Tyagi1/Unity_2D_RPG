using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    private string animBoolName;

    protected Rigidbody2D rb;

    protected bool triggerCalled;
    protected float stateTimer;

    public EnemyState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.enemyBase = _enemyBase;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.animator.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        enemyBase.animator.SetBool(animBoolName, false);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
}
