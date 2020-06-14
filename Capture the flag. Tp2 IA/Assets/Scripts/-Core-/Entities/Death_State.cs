using System.Collections;
using System.Collections.Generic;
using DevTools.Enums;
using UnityEngine;
using UnityEngine.AI;

public class Death_State : Base_State
{
    private float _deathCount = 4;
    public Death_State(Entity smOwner) : base(smOwner)
    {
        
    }

    public override void OnEnter()
    {
        _smOwner.GetComponent<NavMeshAgent>().Warp(_smOwner.initPos);
    }

    public override void Tick()
    {
        _deathCount -= Time.deltaTime;

        if (_deathCount <= 0)
        {
            _deathCount = 4;
            _smOwner.sm.SetState(_smOwner.statesRegistry[Enums.SM_STATES.SearchFlag]);
        }
    }
}
