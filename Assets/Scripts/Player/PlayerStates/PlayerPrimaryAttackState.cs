using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : BasePlayerState
{
    private const string COMBO_COUNTER = "ComboCounter";

    private int comboCounter;

    private float comboWindow = 0.5f;
    private float lastTimeAttacked;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // To Prevent attack direction bug
        xInput = 0;

        // Reset Combo Counter if we have over the max attack count, or if we are out of the combo time window
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow) comboCounter = 0;
        stateTimer = 0.1f;

        player.animator.SetInteger(COMBO_COUNTER, comboCounter);

        player.SetVelocity(player.attackMovements[comboCounter] * GetAttackDirection(), rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0) player.SetZeroVelocity();

        if (triggerCalled) stateMachine.ChangeState(player.idleState);
    }

    private float GetAttackDirection()
    {
        float attackDirection = player.facingDirection;
        if(xInput != 0)
        {
            attackDirection = xInput;
        }

        return attackDirection;
    }
}
