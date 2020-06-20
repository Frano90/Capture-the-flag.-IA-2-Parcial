using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchForFlag_State : Base_State
{
    private float _count = 1;
    private Flag _flag;
    public SearchForFlag_State(Entity smOwner) : base(smOwner)
    {
    }

    public override void OnEnter()
    {
        _flag = Main.instance.gameCotroller.flag;
//        Debug.Log("entro aca");
        //_smOwner.desiredPos = GetPosRandom(40, _smOwner.transform);

        
        //Si encuentro la bandera
        if (Vector3.Distance(_flag.transform.position, _smOwner.transform.position) <= 5)
        {
            _smOwner.knowsWhereFlagIs = true;
            return;
        }
        
        _smOwner.desiredPos = GetPosRandom(40, _smOwner.transform);

    }

    bool CheckIfCanGoToTargetPosition(Vector3 pos)
    {
        NavMeshPath posiblePath = new NavMeshPath();
        
        return  _smOwner.GetComponent<NavMeshAgent>().CalculatePath(pos, posiblePath);
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
