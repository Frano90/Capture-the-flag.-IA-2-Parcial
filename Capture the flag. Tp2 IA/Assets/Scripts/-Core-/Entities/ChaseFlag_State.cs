using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseFlag_State : Base_State
{
    private Flag _flag;
    
    public ChaseFlag_State(Entity smOwner) : base(smOwner)
    {
        _flag = Main.instance.gameCotroller.flag;
    }

    public override void Start()
    {
        
    }
}
