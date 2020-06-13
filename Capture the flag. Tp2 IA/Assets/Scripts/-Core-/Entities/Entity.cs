using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private StateMachine _sm;

    private void Start()
    {
        _sm = new StateMachine();
        
        var chaseFlag = new ChaseFlag_State(this);
        
        _sm.SetState(chaseFlag);
        
    }

    private void Update()
    {
        _sm.Tick();
    }
}
