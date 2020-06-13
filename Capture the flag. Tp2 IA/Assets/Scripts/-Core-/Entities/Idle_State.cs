using System.Collections;
using System.Collections.Generic;
using DevTools.Enums;
using UnityEngine;

public class Idle_State : Base_State
{
    public Idle_State(Entity smOwner) : base(smOwner){}

    public override void OnEnter()
    {
        _smOwner.sm.SetState(_smOwner.statesRegistry[Enums.SM_STATES.Move]);
    }
}
