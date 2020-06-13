using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_State : IState
{
    protected Entity _smOwner;
    protected Base_State(Entity smOwner){_smOwner = smOwner;}
    public virtual void Start(){}
    public virtual void Sleep(){}
    public virtual void Tick(){}
}
