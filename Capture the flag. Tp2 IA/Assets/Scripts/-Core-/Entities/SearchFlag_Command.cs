using System;
using System.Collections;
using System.Collections.Generic;
using DevTools.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

public class SearchFlag_Command : Command_Base
{
    private Flag _flag;

    private float _count = 3;
    public SearchFlag_Command(Brain brain, Action callback) : base(brain, callback)
    {
        _flag = Main.instance.gameCotroller.flag;
    }

    public override void Execute()
    {
        
        Debug.Log("cacaca");
        if (Vector3.Distance(_flag.transform.position, _brain.brainOwner.transform.position) <= 5)
        {
            OnFinishCommand?.Invoke();
            return;
        }
        
        if (Vector3.Distance(_brain.desiredPosToGo, _brain.brainOwner.transform.position) <= .5f)
        {
            _brain.desiredPosToGo = GetPosRandom(3, _brain.brainOwner.transform);

            _count -= Time.deltaTime;

            if (_count <= 0)
            {
                _count = 3;
                _brain.desiredPosToGo = GetPosRandom(3, _brain.brainOwner.transform);
            }
        }
    }

    public override void Init(Brain brain, Action callback)
    {
        if (_brain.desiredPosToGo != null) _brain.desiredPosToGo = GetPosRandom(3, _brain.brainOwner.transform); 
        
        _brain.brainOwner.sm.SetState(_brain.brainOwner.statesRegistry[Enums.SM_STATES.Move]);
    }
    
    Vector3 GetPosRandom(float radio, Transform t)
    {
        Vector3 min = new Vector3(t.position.x - radio, 0, t.position.z - radio);
        Vector3 max = new Vector3(t.position.x + radio, 0, t.position.z + radio);
        return new Vector3(Random.Range(min.x, max.x), 0, Random.Range(min.z, max.z));
    }
}
