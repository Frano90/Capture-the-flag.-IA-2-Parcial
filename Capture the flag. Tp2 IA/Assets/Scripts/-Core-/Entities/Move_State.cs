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

    public override void OnEnter()
    {
        
    }

    public override void Tick()
    {
        
    }

    public override void TickFixedUpdate()
    {
        _navMeshAgent.SetDestination(_flag.transform.position);
    }
}
