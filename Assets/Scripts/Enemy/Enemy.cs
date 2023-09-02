using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine;

    // Start is called before the first frame update
    protected override void Start()
    {
        stateMachine = new EnemyStateMachine();
    }

    // Update is called once per frame
    protected override void Update()
    {
        stateMachine.currentState.Update();
    }
}
