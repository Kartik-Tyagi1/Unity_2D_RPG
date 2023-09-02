using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine;

    [SerializeField] protected LayerMask playerLayerMask;

    [Header("Move Parameters")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    public float playerGetAwayDistance;

    [Header("Attack Parameters")]
    public float attackDistance;
    public float playerDetectionRange;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }


    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();      
    }

    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerDetectionRange, playerLayerMask);
}
