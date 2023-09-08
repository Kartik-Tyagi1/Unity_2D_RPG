using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : BasePlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skillManager.swordSkill.ShowDots(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(Input.GetKeyUp(KeyCode.Mouse1)) 
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
