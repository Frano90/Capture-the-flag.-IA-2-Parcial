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

    public Transform exitBasePos;
    public Transform basePos;
    
    private void Start()
    {
        //Setea la salida
        if (_teamSide == Enums.TeamSide.Blue)
        {
            exitBasePos = Main.instance.gameCotroller.blueExitPos;
            basePos = Main.instance.gameCotroller.blueBasePos;
        }
        else
        {
            exitBasePos = Main.instance.gameCotroller.redExitPos;
            basePos = Main.instance.gameCotroller.redBasePos;
        }
        
        initPos = transform.position;
        
        sm = new StateMachine();
        brain = new Brain(this);
        var idle = new Idle_State(this);
        var move = new Move_State(this);
        
        statesRegistry.Add(Enums.SM_STATES.Idle, idle);
        statesRegistry.Add(Enums.SM_STATES.Move, move);
        
        sm.SetState(idle);
        brain.DoNextCommandInQueue();
    }

    private void Update()
    {
        if (Main.instance.gameCotroller.isGameOn)
        {
            sm.Tick();
            brain.Think();
        }
    }

    public void ResetEntity()
    {
        sm.SetState(statesRegistry[Enums.SM_STATES.Idle]);
        GetComponent<NavMeshAgent>().Warp(initPos);
    }
}
