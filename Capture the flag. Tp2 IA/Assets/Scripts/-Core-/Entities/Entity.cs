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
        
        //Creo los estados
        var idle = new Idle_State(this);
        var move = new Move_State(this);
        var stunned = new Stunned_State(this);
        var searchFlag = new SearchForFlag_State(this);
        var chaseFlag = new ChaseFlag_State(this);
        var carryFlag = new CarryFlagToBase_State(this);
        var protectFlagCarrier = new ProtectFlagCarrier_State(this);
        
        //Los registro para hacer cambios forzados de estados
        statesRegistry.Add(Enums.SM_STATES.Idle, idle);
        statesRegistry.Add(Enums.SM_STATES.Move, move);
        statesRegistry.Add(Enums.SM_STATES.Stunned, stunned);
        statesRegistry.Add(Enums.SM_STATES.SearchFlag, searchFlag);
        statesRegistry.Add(Enums.SM_STATES.ChaseFlag, chaseFlag);
        statesRegistry.Add(Enums.SM_STATES.CarryFlagToBase, carryFlag);
        statesRegistry.Add(Enums.SM_STATES.ProtectFlagCarrier, protectFlagCarrier);

        //Agrego transiciones
        At(searchFlag, move, ImStillSearching());
        At(move, searchFlag, FinishMomevent());
        At(chaseFlag, carryFlag, HasFlag());
        At(carryFlag, searchFlag, ImStillSearching());
        At(stunned, searchFlag, ImStillSearching());
        At(protectFlagCarrier, searchFlag, ImStillSearching());
        
        //Shortcut para agregar transiciones
        void At(IState from, IState to, Func<bool> condition) => sm.AddTransition(from, to, condition);

        //Condiciones
        Func<bool> IsStunned() => () => isStunned;
        Func<bool> ImStillSearching() => () => knowsWhereFlagIs == false && hasFlag == false && isStunned == false;
        Func<bool> FinishMomevent() => () => GetComponent<NavMeshAgent>().velocity == Vector3.zero;
        Func<bool> FindFlag() => () => knowsWhereFlagIs  && hasFlag == false && isStunned == false && sm.CurrentState != protectFlagCarrier;
        Func<bool> HasFlag() => () => hasFlag;

        //Desde cualquier estado
        sm.AddAnyTransition(stunned, IsStunned());
        sm.AddAnyTransition(chaseFlag, FindFlag());
        
        //Arranco la maquina en un estado
        sm.SetState(searchFlag);
    }

    /// <summary>
    /// Configuro los datos de la entidad para con su equipo
    /// </summary>
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
            //Tick de la maquina de estaods
            sm.Tick();

            //Debug
            currentState = sm.CurrentState.ToString();
        }
    }

    /// <summary>
    /// Sirve para que todos vayan a sus posiciones iniciales
    /// </summary>
    public void ResetEntityWithPos()
    {
        sm.SetState(statesRegistry[Enums.SM_STATES.SearchFlag]);
        GetComponent<NavMeshAgent>().Warp(initPos);
        
        knowsWhereFlagIs = false;
        isStunned = false;
        hasFlag = false;
    }

    /// <summary>
    /// Para cuando queres resetear su comportamiento pero no su posicion
    /// </summary>
    public void ResetEntitySM()
    {
        sm.SetState(statesRegistry[Enums.SM_STATES.SearchFlag]);
        knowsWhereFlagIs = false;
        isStunned = false;
        hasFlag = false;
    }
    

    //Mira aca el stun y dsp anda a un estado como el de "protectFlagCarrier". Vas a ver que lo que hago es buscar al que esta cerca del flagcarrier y le hago "entity.stun()"
    //la idea es esa misma, que con el click recorras una lista de lo que esta cerca y de ahi le hagas cosas en "entity.Sarasa()"
    
    /// <summary>
    /// Les hace un stun
    /// </summary>
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
