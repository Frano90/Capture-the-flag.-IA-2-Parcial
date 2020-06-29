using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ProtectFlagCarrier_State : Base_State
{

    private Queries _gridQueries;
    private Entity _flagCarrier;
    private NavMeshAgent _navMeshAgent;
    public ProtectFlagCarrier_State(Entity smOwner) : base(smOwner)
    {
        Debug.Log("llego a entrar aca?");
        _navMeshAgent = _smOwner.GetComponent<NavMeshAgent>();
        _gridQueries = Main.instance.gameCotroller.grid;
    }

    public override void Tick()
    {
        if (Main.instance.gameCotroller.flagHolder == null)
        {
            _smOwner.knowsWhereFlagIs = false;
            return;
        }
        
        var aux = EnemyClosestToMe();
        
        if (aux != null)
        {
            _navMeshAgent.SetDestination(aux.transform.position);
            
            if (Vector3.Distance(aux.transform.position, _smOwner.transform.position) <= 2)
            {
                aux.Stun();
            }
            
            return;
        }

        if (Main.instance.gameCotroller.flagHolder != null)
        {
            var flagHolder = Main.instance.gameCotroller.flagHolder.transform;
            _navMeshAgent.SetDestination(flagHolder.position - flagHolder.forward);
        }
            
    }

    Entity EnemyClosestToMe()
    {
        var selected = _gridQueries.Query(_smOwner.transform, new Vector2(_smOwner.lookRange, _smOwner.lookRange));

        //MIRATE ESTE ORDER BY, RAMA!
        return selected
            .Select(x => x.GetComponent<Entity>())
            .Where(x => x != null && x._teamSide != _smOwner._teamSide && x.isStunned == false)
            .OrderBy(x => Vector3.Distance(x.transform.position, _smOwner.transform.position))
            .FirstOrDefault();
        
    }
    

}
