using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarryFlagToBase_State : Base_State
{
    private NavMeshAgent _navMeshAgent;

    private float speed;
    public CarryFlagToBase_State(Entity smOwner) : base(smOwner)
    {
    }

    public override void OnEnter()
    {
        _smOwner.desiredPos = _smOwner.basePos.position;
        _smOwner.GetComponent<NavMeshAgent>().SetDestination(_smOwner.desiredPos);
        _navMeshAgent = _smOwner.GetComponent<NavMeshAgent>();
        speed = _navMeshAgent.speed;
        _navMeshAgent.speed = speed * .5f;
    }

    public override void OnExit()
    {
        _navMeshAgent.speed = speed;
    }

    public override void Tick()
    {
        if (_navMeshAgent.velocity == Vector3.zero && Vector3.Distance(_smOwner.transform.position, _smOwner.desiredPos) <= 2f)
        {
            _smOwner.knowsWhereFlagIs = false;
            _smOwner.hasFlag = false;
            Main.instance.gameCotroller.flag.transform.parent = null;
            Main.instance.gameCotroller.flag.ResetPos();
            Main.instance.gameCotroller.flagHolder = null;
            Main.instance.gameCotroller.OnFlagToBase(_smOwner);

        }
    }
}
