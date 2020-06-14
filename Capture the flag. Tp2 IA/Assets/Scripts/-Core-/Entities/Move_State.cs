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
        _smOwner.desiredPos = Vector3.zero;
        //_smOwner.brain.desiredPosToGo = default;
    }

    public override void Tick()
    {
        //_navMeshAgent.SetDestination(_smOwner.brain.desiredPosToGo);

        _navMeshAgent.SetDestination(_smOwner.desiredPos);
        
        if (Vector3.Distance(_flag.transform.position, _smOwner.transform.position) <= 5)
        {
            _smOwner.knowsWhereFlagIs = true;
            return;
        }
        
        if (Vector3.Distance(_smOwner.transform.position, _smOwner.desiredPos) <= 2)
        {
            _navMeshAgent.velocity = Vector3.zero;
        }
        
    }

    public override void TickFixedUpdate()
    {
        
    }
}
