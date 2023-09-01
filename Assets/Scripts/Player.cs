using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Player : MonoBehaviour
{
    #region Animation Parameters
    private const string IDLE_STATE = "Idle";
    private const string MOVE_STATE = "Move";
    private const string JUMP = "Jump";
    private const string DASH = "Dash";
    private const string WALL_SLIDE = "Wallslide";
    private const string ATTACK_PRIMARY = "Attack";
    #endregion

    #region States
    public  PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallslideState wallslideState { get; private set; }
    public PlayerWallJumpState walljumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }

    #endregion

    #region Components
    public Animator animator {  get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    public bool isBusy { get; private set; }

    [Header("Move Parameters")]
    public float moveSpeed = 8f;
    public float jumpForce;
    public int facingDirection { get; private set; } = 1; // 1 is Right, -1 is Left
    private bool facingRight = true;

    [Header("Collision Parameters")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask groundMask;

    [Header("Dash Parameters")]
    public float dashSpeed = 25f;
    public float dashDuration = 0.2f;
    public float dashDirection;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashUsageTimer;

    [Header("Attack Parameters")]
    public float[] attackMovements;



    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();


        idleState = new PlayerIdleState(this, stateMachine, IDLE_STATE);
        moveState = new PlayerMoveState(this, stateMachine, MOVE_STATE);
        jumpState = new PlayerJumpState(this, stateMachine, JUMP);
        airState = new PlayerAirState(this, stateMachine, JUMP);
        dashState = new PlayerDashState(this, stateMachine, DASH);
        wallslideState = new PlayerWallslideState(this, stateMachine, WALL_SLIDE);
        walljumpState = new PlayerWallJumpState(this, stateMachine, JUMP);

        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, ATTACK_PRIMARY);
    }

    void Start()
    {
        stateMachine.Initialize(idleState);
    }

    void Update()
    {
        stateMachine.currentState.Update();
        CheckForDashInput();
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    // Move to dash state from here cause player should be able to dash from any state
    private void CheckForDashInput()
    {
        if (IsWallDetected()) return;

        dashUsageTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            dashDirection = Input.GetAxisRaw("Horizontal");
            if (dashDirection == 0) dashDirection = facingDirection;
            stateMachine.ChangeState(dashState);
        }
    }

    #region Velocity
    public void SetVelocity(float _xVeloctiy, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVeloctiy, _yVelocity);
        FlipController(_xVeloctiy);
    }

    public void ZeroVelocity() => rb.velocity = new Vector2(0, 0);
    #endregion

    #region Collision
    public bool IsOnGround() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundMask);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, groundMask);
    #endregion

    #region Flip
    public void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController(float _x)
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y ));
    }

}
