using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command_Base
{
    protected float delay = .2f;
    protected Brain _brain;
    protected Action OnFinishCommand;
    public Command_Base(Brain brain, Action callback){_brain = brain;OnFinishCommand = callback;}

    public abstract void Execute();
    public abstract void Init(Brain brain, Action callback);
    
}
