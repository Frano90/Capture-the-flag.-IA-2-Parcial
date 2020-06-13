using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevTools.Enums;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    public StateMachine sm;
    public Brain brain;
    public Vector3 initPos;

    [SerializeField] private Enums.TeamSide _teamSide;
    
    public Dictionary<Enums.SM_STATES, Base_State> statesRegistry = new Dictionary<Enums.SM_STATES, Base_State>();

    private void Start()
    {
        initPos = transform.position;
        
        sm = new StateMachine();
        brain = new Brain(this);
        var idle = new Idle_State(this);
        var move = new Move_State(this);
        
        statesRegistry.Add(Enums.SM_STATES.Idle, idle);
        statesRegistry.Add(Enums.SM_STATES.Move, move);
        
        sm.SetState(idle);
        //brain.DoNextCommandInQueue();
    }

    private void Update()
    {
        if (Main.instance.gameCotroller.isGameOn)
        {
            sm.Tick();
            //brain.Think();
        }
    }

    public void ResetEntity()
    {
        sm.SetState(statesRegistry[Enums.SM_STATES.Idle]);
        GetComponent<NavMeshAgent>().Warp(initPos);
    }
}
