using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Player : Entity
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

    public bool isBusy { get; private set; }

    [Header("Move Parameters")]
    public float moveSpeed = 8f;
    public float jumpForce;


    [Header("Dash Parameters")]
    public float dashSpeed = 25f;
    public float dashDuration = 0.2f;
    public float dashDirection;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashUsageTimer;

    [Header("Attack Parameters")]
    public float[] attackMovements;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, IDLE_STATE);
        moveState = new PlayerMoveState(this, stateMachine, MOVE_STATE);
        jumpState = new PlayerJumpState(this, stateMachine, JUMP);
        airState = new PlayerAirState(this, stateMachine, JUMP);
        dashState = new PlayerDashState(this, stateMachine, DASH);
        wallslideState = new PlayerWallslideState(this, stateMachine, WALL_SLIDE);
        walljumpState = new PlayerWallJumpState(this, stateMachine, JUMP);

        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, ATTACK_PRIMARY);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
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
}
