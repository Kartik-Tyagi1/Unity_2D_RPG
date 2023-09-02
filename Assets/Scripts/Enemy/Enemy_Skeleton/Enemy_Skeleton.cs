using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region Animation Parameters
    private const string IDLE_STATE = "Idle";
    private const string MOVE_STATE = "Move";
    private const string ATTACK_STATE = "Attack";
    private const string STUNNED_STATE = "Stunned";
    #endregion

    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SkeletonIdleState(stateMachine, this, IDLE_STATE, this);
        moveState = new SkeletonMoveState(stateMachine, this, MOVE_STATE, this);
        battleState = new SkeletonBattleState(stateMachine, this, MOVE_STATE, this);
        attackState = new SkeletonAttackState(stateMachine, this, ATTACK_STATE, this);
        stunnedState = new SkeletonStunnedState(stateMachine, this, STUNNED_STATE, this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    // Lets player see if the enemy can be stunned in the counter window
    public override bool CanBeStunned()
    {
        if(base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }
}
