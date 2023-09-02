using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    private Player player;
    public BasePlayerState currentState { get; private set; }
   

    public void Initialize(BasePlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(BasePlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
