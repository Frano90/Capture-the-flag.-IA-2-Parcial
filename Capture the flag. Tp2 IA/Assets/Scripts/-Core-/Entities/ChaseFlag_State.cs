using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseFlag_State : Base_State
{
    private Flag _flag;
    private NavMeshAgent _navMeshAgent;
    
    public ChaseFlag_State(Entity smOwner) : base(smOwner)
    {
    }

    public override void OnEnter()
    {
        _flag = Main.instance.gameCotroller.flag;
        _navMeshAgent = _smOwner.GetComponent<NavMeshAgent>();

        
    }

    public override void Tick()
    {
        _smOwner.desiredPos = _flag.transform.position;
        
        _navMeshAgent.SetDestination(_smOwner.desiredPos);
        
        //Si encuentro la bandera
        if (Vector3.Distance(_flag.transform.position, _smOwner.transform.position) <= 2)
        {
            if (Main.instance.gameCotroller.flagHolder == null )
            {
                Main.instance.gameCotroller.flagHolder = _smOwner;
                _flag.transform.SetParent(_smOwner.transform);
                _smOwner.hasFlag = true;    
            }
            else if(Main.instance.gameCotroller.flagHolder._teamSide != _smOwner._teamSide)
            {
                Main.instance.gameCotroller.flagHolder.Stun();
            }
            
        }
    }
}
