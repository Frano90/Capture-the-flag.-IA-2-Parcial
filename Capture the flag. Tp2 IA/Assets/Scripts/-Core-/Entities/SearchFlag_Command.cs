using System;
using System.Collections;
using System.Collections.Generic;
using DevTools.Enums;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SearchFlag_Command : Command_Base
{
    private Flag _flag;

    private float _count = 1;
    public SearchFlag_Command(Brain brain, Action callback) : base(brain, callback)
    {
        _flag = Main.instance.gameCotroller.flag;
    }

    public override void Execute()
    {
        //Si encuentro la bandera
        if (Vector3.Distance(_flag.transform.position, _brain.brainOwner.transform.position) <= 5)
        {
            OnFinishCommand?.Invoke();
            return;
        }
        
        
        if (Vector3.Distance(_brain.desiredPosToGo, _brain.brainOwner.transform.position) <= 1f)
        {
            _brain.desiredPosToGo = GetPosRandom(40, _brain.brainOwner.transform);
        }
        
        if(_brain.brainOwner.GetComponent<NavMeshAgent>().velocity.Equals(Vector3.zero))
        {
            _count -= Time.deltaTime;
            if (_count <= 0)
            {
                _count = 1;
                _brain.desiredPosToGo = GetPosRandom(40, _brain.brainOwner.transform);
            }
        }
    }

    public override void Init(Brain brain, Action callback)
    {
        _brain.desiredPosToGo = GetPosRandom(40, _brain.brainOwner.transform); 
        
        _brain.brainOwner.sm.SetState(_brain.brainOwner.statesRegistry[Enums.SM_STATES.Move]);
    }

    bool CheckIfCanGoToTargetPosition(Vector3 pos)
    {
        NavMeshPath posiblePath = new NavMeshPath();
        
        return  _brain.brainOwner.GetComponent<NavMeshAgent>().CalculatePath(pos, posiblePath);
    }
    
    Vector3 GetPosRandom(float radio, Transform t)
    {
        Vector3 min = new Vector3(t.position.x - radio, 0, t.position.z - radio);
        Vector3 max = new Vector3(t.position.x + radio, 0, t.position.z + radio);
        
        Vector3 newPos = new Vector3(Random.Range(min.x, max.x), 0, Random.Range(min.z, max.z));

        if (CheckIfCanGoToTargetPosition(newPos))
        {
            return newPos;
        }
        else
        { 
            GetPosRandom(radio, t);
            return t.transform.position;
        }
    }
}
