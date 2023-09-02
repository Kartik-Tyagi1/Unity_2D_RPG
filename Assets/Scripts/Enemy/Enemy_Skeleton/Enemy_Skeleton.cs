using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region Animation Parameters
    private const string IDLE_STATE = "Idle";
    private const string MOVE_STATE = "Move";
    #endregion

    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SkeletonIdleState(stateMachine, this, IDLE_STATE, this);
        moveState = new SkeletonMoveState(stateMachine, this, MOVE_STATE, this);
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
}
