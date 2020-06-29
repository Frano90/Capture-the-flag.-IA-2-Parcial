using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DevTools.Enums;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System;
using UnityEngine.UI;

public class Queries : MonoBehaviour
{
    public bool isBox;
    public float radius = 20f;
    public SpatialGrid targetGrid;
    public float width = 15f;
    public float height = 30f;
    public IEnumerable<GridEntity> selected = new List<GridEntity>();
    public float rayLength;
    public LayerMask layerMask;
    public bool activeGizmos;
    public Animator myAnim;
    public List<Button> buttonsExplosionSelectTeamToHelp;

    public Enums.TeamToHelp whatTeamAffectExplosion;

    public enum ClickPower
    {
        DoubleSlow /*concat*/
       , StunIfNotHaveFlag
       , Explosion
    };

    public ClickPower clickPower;

    public void ActualPower(int actualPower)
    {
        switch (actualPower)
        {
            case 0:
                {
                    clickPower = ClickPower.DoubleSlow;
                    break;
                }
            case 1:
                {
                    clickPower = ClickPower.StunIfNotHaveFlag;
                    break;
                }
            case 2:
                {
                    clickPower = ClickPower.Explosion;
                    foreach (var item in buttonsExplosionSelectTeamToHelp)
                    {
                        item.gameObject.SetActive(true);
                    }
                    break;
                }
        }
    }

    public void TeamToHelp(int helpTeam)
    {
        switch (helpTeam)
        {
            case 0:
                {
                    whatTeamAffectExplosion = Enums.TeamToHelp.Blue;
                    break;
                }
            case 1:
                {
                    whatTeamAffectExplosion = Enums.TeamToHelp.Red;
                    break;
                }
        }
    }

    public void RayFromCamera(bool cooldown)
    {
        if (!cooldown)
        {
            RaycastHit hit;
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, rayLength, layerMask))
                {
                    activeGizmos = true;
                    this.transform.position = hit.point;
                    //var clickPos = this.transform;
                    //Query();
                    myAnim.SetBool("onClick", true);
                    selected = Query();
                    Exec1();
                }
            }
        }
    }

    public void StopCallClickAnim()
    {
        myAnim.SetBool("onClick", false);
    }

    //IA2-P2
    public IEnumerable<GridEntity> Query()
    {
        if (isBox)
        {
            var h = height * 0.5f;
            var w = width * 0.5f;
            //posicion inicial --> esquina superior izquierda de la "caja"
            //posición final --> esquina inferior derecha de la "caja"
            //como funcion para filtrar le damos una que siempre devuelve true, para que no filtre nada.
            return targetGrid.Query(
                transform.position + new Vector3(-w, 0, -h),
                transform.position + new Vector3(w, 0, h),
                x => true);
        }
        else
        {
            //creo una "caja" con las dimensiones deseadas, y luego filtro segun distancia para formar el círculo
            return targetGrid.Query(
                transform.position + new Vector3(-radius, 0, -radius),
                transform.position + new Vector3(radius, 0, radius),
                x =>
                {
                    var position2d = x - transform.position;
                    position2d.y = 0;
                    return position2d.sqrMagnitude < radius * radius;
                });
        }
    }

    public IEnumerable<GridEntity> Query(Transform clickPos, Vector2 rango)
    {
        if (isBox)
        {
            var h = rango.y * 0.5f;
            var w = rango.x * 0.5f;
            //posicion inicial --> esquina superior izquierda de la "caja"
            //posición final --> esquina inferior derecha de la "caja"
            //como funcion para filtrar le damos una que siempre devuelve true, para que no filtre nada.
            return targetGrid.Query(
                clickPos.position + new Vector3(-w, 0, -h),
                clickPos.position + new Vector3(w, 0, h),
                x => true);
        }
        else
        {
            //creo una "caja" con las dimensiones deseadas, y luego filtro segun distancia para formar el círculo
            return targetGrid.Query(
                clickPos.position + new Vector3(-radius, 0, -radius),
                clickPos.position + new Vector3(radius, 0, radius),
                x =>
                {
                    var position2d = x - transform.position;
                    position2d.y = 0;
                    return position2d.sqrMagnitude < radius * radius;
                });
        }
    }

    void OnDrawGizmos()
    {
        if (activeGizmos)
        {
            if (targetGrid == null)
                return;

            //Flatten the sphere we're going to draw
            Gizmos.color = Color.cyan;
            if (isBox)
                Gizmos.DrawWireCube(transform.position, new Vector3(width, 0, height));
            else
            {
                Gizmos.matrix *= Matrix4x4.Scale(Vector3.forward + Vector3.right);
                Gizmos.DrawWireSphere(transform.position, radius);
            }

            if (Application.isPlaying)
            {
                selected = Query();
                var temp = FindObjectsOfType<GridEntity>().Where(x => !selected.Contains(x));
                //var temp = targetGrid.entidadesEnGrilla.Where(x => !selected.Contains(x));
                foreach (var item in temp)
                {
                    item.onGrid = false;
                }
                foreach (var item in selected)
                {
                    //aca va la explosion
                    //item.getcomp<Entity>().stun();
                    item.onGrid = true;
                }
                activeGizmos = false;
            }
        }
    }

    IEnumerable<Entity> _firstSelection = new List<Entity>();
    IEnumerable<Entity> _secondSelection = new List<Entity>();
    bool firstSlow = true;
    bool helpBlueTeam;

    //IA2-P2
    void Exec1()
    {
        switch (clickPower)
        {
            case ClickPower.DoubleSlow:
                {
                    Debug.Log("DOBLESLOW");
                    if (firstSlow)
                    {
                        _firstSelection = selected.Select(x => x.GetComponent<Entity>()).Where(x => x != null);
                        foreach (var item in _firstSelection)
                        {
                            //item.myParticlesEntity[0].Play();                               
                        }
                        firstSlow = false;
                    }
                    else
                    {
                        _secondSelection = selected.Select(x => x.GetComponent<Entity>()).Where(x => x != null);
                        var bothSelections = _firstSelection.Concat(_secondSelection);
                        foreach (var item in bothSelections)
                        {
                            //item.myParticlesEntity[0].Play();
                            item.Slow();
                        }
                        firstSlow = true;
                    }
                    break;
                }
            case ClickPower.StunIfNotHaveFlag:
                {
                    Debug.Log("STUNSINOTENGOBANDERA");
                    //Aca hay selec y where RAAAAMAAAA
                    var entities = selected.Select(x => x.GetComponent<Entity>()).Where(x => x != null).Where(x => !x.hasFlag);
                    Debug.Log(entities.Count());
                    foreach (var item in entities)
                    {
                        item.Stun();
                    }
                    break;
                }
            case ClickPower.Explosion:
                {
                    //RAMAAAA ACA ESTA EL AGREGATE QUE ES UN WHERE ENCUBIERTO
                    Debug.Log("EXPLOSION");
                    var entitiesToExplode = selected.Select(x => x.GetComponent<Entity>()).Where(x => x != null).Aggregate(new List<Entity>(), (acum, current) =>
                    {
                        if (current._teamSide == Enums.TeamSide.Blue && whatTeamAffectExplosion == Enums.TeamToHelp.Blue)
                        {
                            acum.Add(current);
                        }
                        else if (current._teamSide == Enums.TeamSide.Red && whatTeamAffectExplosion == Enums.TeamToHelp.Red)
                        {
                            acum.Add(current);
                        }
                        return acum;
                    });
                    foreach (var item in entitiesToExplode)
                    {
                        item.Explosion(transform, false);
                    }
                    break;
                }
        }
    }
}

