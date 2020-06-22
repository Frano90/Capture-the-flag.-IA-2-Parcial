using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevTools.Enums;
using UnityEngine.AI;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    public StateMachine sm;
    public Brain brain;
    public Vector3 initPos;

    public Enums.TeamSide _teamSide;
    public float lookRange;
    
    public Dictionary<Enums.SM_STATES, Base_State> statesRegistry = new Dictionary<Enums.SM_STATES, Base_State>();

    //Debug
    public Text debugState;
    public string currentState;
    
    public Transform exitBasePos;
    public Transform basePos;
    
    //State
    public bool isStunned = false;
    public Vector3 desiredPos;
    public bool knowsWhereFlagIs;
    public bool hasFlag;
    private void Start()
    {
        
        SetTeamData();
        
        initPos = transform.position;
        
        sm = new StateMachine();
        //brain = new Brain(this);
        var idle = new Idle_State(this);
        var move = new Move_State(this);
        var stunned = new Stunned_State(this);
        var searchFlag = new SearchForFlag_State(this);
        var chaseFlag = new ChaseFlag_State(this);
        var carryFlag = new CarryFlagToBase_State(this);
        var protectFlagCarrier = new ProtectFlagCarrier_State(this);
        
        
        statesRegistry.Add(Enums.SM_STATES.Idle, idle);
        statesRegistry.Add(Enums.SM_STATES.Move, move);
        statesRegistry.Add(Enums.SM_STATES.Stunned, stunned);
        statesRegistry.Add(Enums.SM_STATES.SearchFlag, searchFlag);
        statesRegistry.Add(Enums.SM_STATES.ChaseFlag, chaseFlag);
        statesRegistry.Add(Enums.SM_STATES.CarryFlagToBase, carryFlag);
        statesRegistry.Add(Enums.SM_STATES.ProtectFlagCarrier, protectFlagCarrier);

        
        At(searchFlag, move, ImStillSearching());
        At(move, searchFlag, FinishMomevent());
        At(chaseFlag, carryFlag, HasFlag());
        At(carryFlag, searchFlag, ImStillSearching());
        At(stunned, searchFlag, ImStillSearching());
        
        //At(chaseFlag, protectFlagCarrier, ProtectCarrier());
        At(protectFlagCarrier, searchFlag, ImStillSearching());
        
        
        void At(IState from, IState to, Func<bool> condition) => sm.AddTransition(from, to, condition);

        
        Func<bool> IsStunned() => () => isStunned;
        Func<bool> ImStillSearching() => () => knowsWhereFlagIs == false && hasFlag == false && isStunned == false;
        Func<bool> FinishMomevent() => () => GetComponent<NavMeshAgent>().velocity == Vector3.zero;
        Func<bool> FindFlag() => () => knowsWhereFlagIs  && hasFlag == false && isStunned == false && sm.CurrentState != protectFlagCarrier;
        Func<bool> HasFlag() => () => hasFlag;

//        Func<bool> ProtectCarrier() => () =>
//            (knowsWhereFlagIs && hasFlag == false && isStunned == false &&
//             Main.instance.gameCotroller.flagHolder._teamSide == _teamSide); 
        
        
        sm.AddAnyTransition(stunned, IsStunned());
        sm.AddAnyTransition(chaseFlag, FindFlag());
        
        
        sm.SetState(searchFlag);
    }

    void SetTeamData()
    {
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
    }

    private void Update()
    {
        if (Main.instance.gameCotroller.isGameOn)
        {
            sm.Tick();
           // brain.Think();

           currentState = sm.CurrentState.ToString();
        }
    }

    public void ResetEntityWithPos()
    {
        sm.SetState(statesRegistry[Enums.SM_STATES.SearchFlag]);
        GetComponent<NavMeshAgent>().Warp(initPos);
    }

    public void ResetEntitySM()
    {
        sm.SetState(statesRegistry[Enums.SM_STATES.SearchFlag]);
        knowsWhereFlagIs = false;
        isStunned = false;
        hasFlag = false;
    }
    

    public void Stun()
    {
        isStunned = true;
        Main.instance.gameCotroller.flagHolder = null;
        hasFlag = false;
        knowsWhereFlagIs = false;
        //sm.SetState(statesRegistry[Enums.SM_STATES.Stunned]);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRange);
    }
}
