using System;
using System.Collections;
using System.Collections.Generic;
using DevTools.Enums;
using UnityEngine;

public class ReturnWithFlag_Command : Command_Base
{
    public ReturnWithFlag_Command(Brain brain, Action callback) : base(brain, callback)
    {
    }

    public override void Execute()
    {
        
    }

    public override void Init(Brain brain, Action callback)
    {
        Main.instance.gameCotroller.flag.transform.SetParent(_brain.brainOwner.transform);
        
        _brain.desiredPosToGo = _brain.brainOwner.basePos.position;
        _brain.brainOwner.sm.SetState(_brain.brainOwner.statesRegistry[Enums.SM_STATES.Move]);
    }
}
