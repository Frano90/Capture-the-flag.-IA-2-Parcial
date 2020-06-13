using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevTools.Enums;

public class Entity : MonoBehaviour
{
    public StateMachine sm;

    [SerializeField] private Enums.TeamSide _teamSide;
    
    public Dictionary<Enums.SM_STATES, Base_State> statesRegistry = new Dictionary<Enums.SM_STATES, Base_State>();

    private void Start()
    {
        sm = new StateMachine();
        
        var idle = new Idle_State(this);
        var move = new Move_State(this);
        
        statesRegistry.Add(Enums.SM_STATES.Idle, idle);
        statesRegistry.Add(Enums.SM_STATES.Move, move);
        
        sm.SetState(idle);
        
    }

    private void Update()
    {
        if(Main.instance.gameCotroller.IsGameOn)
            sm.Tick();
    }
}
