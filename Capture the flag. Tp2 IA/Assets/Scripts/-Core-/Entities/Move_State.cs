using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move_State : Base_State
{
    private Flag _flag;
    private NavMeshAgent _navMeshAgent;
    
    public Move_State(Entity smOwner) : base(smOwner)
    {
        _flag = Main.instance.gameCotroller.flag;
        _navMeshAgent = _smOwner.GetComponent<NavMeshAgent>();
        
    }

    public override void OnExit()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath();
    }

    public override void Tick()
    {
        //_navMeshAgent.SetDestination(_smOwner.brain.desiredPosToGo);
        _navMeshAgent.SetDestination(_flag.transform.position);
    }

    public override void TickFixedUpdate()
    {
        
    }
}
