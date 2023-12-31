using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX entityFX { get; private set; }
    #endregion

    [Header("Collision Parameters")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask groundMask;

    [Header("Attack Parameters")]
    public Transform attackCheck;
    public float attackCheckRadius;

    [Header("KnockBack Parameters")]
    public Vector2 knockBackDirection;
    private bool isKnockedBack;
    public float knockBackTime;

    public int facingDirection { get; private set; } = 1; // 1 is Right, -1 is Left
    protected bool facingRight = true;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        entityFX = GetComponent<EntityFX>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public virtual void Damage()
    {
        Debug.Log(gameObject.name + " Was Attacked");
        entityFX.StartCoroutine("FlashFX");
        StartCoroutine("KnockBack");
    }

    private IEnumerator KnockBack()
    {
        isKnockedBack = true;

        rb.velocity = new Vector2(knockBackDirection.x * -facingDirection, knockBackDirection.y);

        yield return new WaitForSeconds(knockBackTime);

        isKnockedBack = false;
    }

    #region Collision

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundMask);

    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, groundMask);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    #endregion

    #region Flip

    public virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }

    #endregion

    #region Velocity
    public void SetVelocity(float _xVeloctiy, float _yVelocity)
    {
        if (isKnockedBack) return;

        rb.velocity = new Vector2(_xVeloctiy, _yVelocity);
        FlipController(_xVeloctiy);
    }

    public void SetZeroVelocity()
    {
        if (isKnockedBack) return;
        rb.velocity = new Vector2(0, 0);
    }
    #endregion
}
