using System.Collections;
using System.Collections.Generic;
using DevTools.Enums;
using UnityEngine;
using UnityEngine.AI;

public class Idle_State : Base_State
{
    public Idle_State(Entity smOwner) : base(smOwner){}
    
    private float _count = 2;

    public override void OnEnter()
    {

    }

    public override void Tick()
    {
        //_count -= Time.deltaTime;

//        if (_count <= 0)
//        {
//            _count = 2;
//            _smOwner.sm.SetState(_smOwner.statesRegistry[Enums.SM_STATES.Move]);
//        }
    }
}
