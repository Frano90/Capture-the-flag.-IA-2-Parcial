using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ProtectFlagCarrier_State : Base_State
{

    private Queries _gridQueries;
    private SpatialGrid _grid;
    private Entity _flagCarrier;
    private NavMeshAgent _navMeshAgent;
    public ProtectFlagCarrier_State(Entity smOwner) : base(smOwner)
    {
        _navMeshAgent = _smOwner.GetComponent<NavMeshAgent>();
        _gridQueries = Main.instance.gameCotroller.grid;
        _grid = Main.instance.gameCotroller.grid.GetComponent<SpatialGrid>();

        _flagCarrier = Main.instance.gameCotroller.flagHolder;
    }

    public override void Tick()
    {
        if (Main.instance.gameCotroller.flagHolder == null)
        {
            _smOwner.knowsWhereFlagIs = false;
            //_smOwner.teamHasdFlag = false;
            return;
        }
        
        var aux = ClosestToMe();
        if (aux != null)
        {
            _navMeshAgent.SetDestination(aux.transform.position);
            
            if (Vector3.Distance(aux.transform.position, _smOwner.transform.position) <= 2)
            {
                aux.Stun();
            }
            
            return;
        }
        
        _navMeshAgent.SetDestination(_flagCarrier.transform.position);
    }

    Entity ClosestToMe()
    {
        var selected = _gridQueries.Query(_smOwner.transform, new Vector2(_smOwner.lookRange, _smOwner.lookRange));

        return  selected.OfType<Entity>().Where(x => x._teamSide != _smOwner._teamSide && x.isStunned == false).
            OrderBy(x => Vector3.Distance(x.transform.position, _smOwner.transform.position)).First();
        
        

    }
    
    


}
