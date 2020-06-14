using System;
using System.Collections;
using System.Collections.Generic;
using DevTools.Enums;
using UnityEngine;

public class GetOutOfBase_Command : Command_Base
{
    public GetOutOfBase_Command(Brain brain, Action callback) : base(brain, callback)
    {
    }

    public override void Execute()
    {
        if (Vector3.Distance(_brain.desiredPosToGo, _brain.brainOwner.transform.position) <= 4)
        {
            OnFinishCommand?.Invoke();
            return;
        }
    }

    public override void Init(Brain brain, Action callback)
    {
        _brain.desiredPosToGo = _brain.brainOwner.exitBasePos.position;
        _brain.brainOwner.sm.SetState(_brain.brainOwner.statesRegistry[Enums.SM_STATES.Move]);
    }
}
