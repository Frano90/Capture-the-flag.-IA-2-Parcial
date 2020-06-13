using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    IState current;
    IState previous;
    
    List<IState> stateList = new List<IState>();

    public void Update()
    {
        if (current != null)
        {
            current.Tick();
        }
    }
    public void Addstate(IState state)
    {
        stateList.Add(state);
        if (current == null)
        {
            current = state;
        }
    }
    public void ChangeState<T>() where T : IState
    {
        for (int i = 0; i < stateList.Count; i++)
        {
            if (stateList[i].GetType() == typeof(T))
            {
                current.Sleep();
                current = stateList[i];
                current.Start();
            }
        }
    }
    public bool IsActualState<T>() where T : IState
    {
        return current.GetType() == typeof(T);
    }
}
