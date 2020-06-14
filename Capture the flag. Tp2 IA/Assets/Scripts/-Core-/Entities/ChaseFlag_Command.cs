using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseFlag_Command : Command_Base
{
    private Flag _flag;
    
    public ChaseFlag_Command(Brain brain, Action callback) : base(brain, callback)
    {
        
    }

    public override void Execute()
    {
        _brain.desiredPosToGo = _flag.transform.position;

        if (Vector3.Distance(_brain.desiredPosToGo, _brain.brainOwner.transform.position) <= .3f)
        {
            OnFinishCommand?.Invoke();
            return;
        }
    }

    public override void Init(Brain brain, Action callback)
    {
        _flag = Main.instance.gameCotroller.flag;
        _brain.desiredPosToGo = _flag.transform.position;
    }
}
