using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Base class for all types of player states
 * This class allows each state to have specific functionality for all things the player can do in each state
 * Also handles functionality for what should happen when the player enters, is in , and exits the state
 */

public class BasePlayerState
{
    private const string Y_VELOCITY = "yVelocity";

    protected float xInput;
    protected float yInput;

    protected Player player;
    protected PlayerStateMachine stateMachine;
    private string animBoolName;
    protected Rigidbody2D rb;

    // timer that can be used by individual states
    protected float stateTimer;

    // trigger that individual states can use for animation triggers
    protected bool triggerCalled;


    public BasePlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.animator.SetBool(animBoolName, true);
        rb = player.rb;

        triggerCalled = false;
    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.animator.SetFloat(Y_VELOCITY, rb.velocity.y);
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        player.animator.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }


}
