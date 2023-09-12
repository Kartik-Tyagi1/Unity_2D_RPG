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
    private float defaultMoveSpeed;

    [Header("Attack Parameters")]
    public float attackDistance;
    public float playerDetectionRange;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    [Header("Stunned Paremeters")]
    public float stunnedTime;
    public Vector2 stunnedDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }


    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();   
    }

    public virtual void FreezeEnemy(bool _timeFrozen)
    {
        if(_timeFrozen)
        {
            moveSpeed = 0f;
            animator.speed = 0f;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            animator.speed = 1f;
        }
    }

    protected virtual IEnumerator FreezeEnemyTimer(float _seconds)
    {
        FreezeEnemy(true);
        yield return new WaitForSeconds(_seconds);
        FreezeEnemy(false);
    }

    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual void TurnOnCounterImage()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void TurnOffCounterImage()
    {
        canBeStunned= false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if(canBeStunned)
        {
            TurnOffCounterImage();
            return true;
        }

        return false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerDetectionRange, playerLayerMask);
}
