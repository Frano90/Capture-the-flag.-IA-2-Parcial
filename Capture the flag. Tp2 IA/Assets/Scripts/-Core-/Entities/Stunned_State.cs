using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stunned_State : Base_State
{
    private float stunnedTime = 3f;
    
    public Stunned_State(Entity smOwner) : base(smOwner)
    {
        
    }

    public override void OnEnter()
    {
//        _smOwner.brain.InterruptThink();
        _smOwner.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        _smOwner.GetComponent<NavMeshAgent>().ResetPath();
    }

    public override void OnExit()
    {
        
  //      _smOwner.brain.ResumeThink();
    }

    public override void Tick()
    {
        stunnedTime -= Time.deltaTime;

        if (stunnedTime <= 0)
        {
            stunnedTime = 3f;
            _smOwner.isStunned = false;
            //_smOwner.sm.SetState(_smOwner.statesRegistry[]);
        }
    }
}
